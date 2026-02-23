using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ECS
{
    public class EntityManager
    {
        public const int ENTITY_BUCKET_SIZE = 64;
        private Entity[] _entities;
        private int _totalEntityCount;
        private Stack<int> _freeEntityIndexStack;
        private List<int> _activeEntityIndexStack;
        private int _activeEntityCount = 0;
        private List<int> _indexInActiveEntityIndexStack;
        public int ActiveEntityCount => _activeEntityCount;
        public int InActiveEntityCount => _freeEntityIndexStack.Count;
        public int TotalEntityCount => _totalEntityCount;
        public EntityManager() => Init();
        public void Init()
        {
            _entities = new Entity[ENTITY_BUCKET_SIZE];
            _freeEntityIndexStack = new();
            _activeEntityIndexStack = new(ENTITY_BUCKET_SIZE);
            _indexInActiveEntityIndexStack = new(ENTITY_BUCKET_SIZE);
            for (int i = 0; i < ENTITY_BUCKET_SIZE; i++)
            {
                _freeEntityIndexStack.Push(i);
                _indexInActiveEntityIndexStack.Add(-1);
                _entities[i].Set((uint)i + 1, -1, 0, 0);
            }
            _totalEntityCount = ENTITY_BUCKET_SIZE;
        }
        public void Expand()
        {
            int oldTotal = _totalEntityCount;
            int newTotal = oldTotal + ENTITY_BUCKET_SIZE;
            Array.Resize(ref _entities, TotalEntityCount + ENTITY_BUCKET_SIZE);
            _activeEntityIndexStack.Capacity += ENTITY_BUCKET_SIZE;
            _indexInActiveEntityIndexStack.Capacity += ENTITY_BUCKET_SIZE;
            for (int slot = oldTotal; slot < newTotal; slot++)
            {
                _freeEntityIndexStack.Push(slot);
                _indexInActiveEntityIndexStack.Add(-1);
                _entities[slot].Set((uint)slot + 1, -1, 0, 0);
            }
            _totalEntityCount = newTotal;
        }
        public Entity GetEntity(int gameObjectID)
        {
            if (_freeEntityIndexStack.Count == 0)
                Expand();
            int index = _freeEntityIndexStack.Pop();
            if (_activeEntityIndexStack.Count > _activeEntityCount)
                _activeEntityIndexStack[_activeEntityCount] = index;
            else
                _activeEntityIndexStack.Add(index);
            _indexInActiveEntityIndexStack[index] = _activeEntityCount++;
            _entities[index].Set((uint)index + 1, gameObjectID, _entities[index].Version, 0);
            return _entities[index];
        }
        public void ReleaseEntity(in Entity entity)
        {
            int id = (int)entity.EntityID - 1;
            if (id < 0 || id >= TotalEntityCount)
            {
                Debug.LogError($"EntityManager ReleaseEntity Error:Index:{id} Out Of Range [1,{TotalEntityCount})");
                return;
            }
            int pos = _indexInActiveEntityIndexStack[id];
            if (pos < 0)
            {
                Debug.LogError($"EntityManager ReleaseEntity Error:Entity Is Not Active");
                return;
            }
            int activePos = _activeEntityIndexStack[--_activeEntityCount];
            _activeEntityIndexStack[pos] = activePos;
            _indexInActiveEntityIndexStack[activePos] = pos;
            _indexInActiveEntityIndexStack[id] = -1;
            _entities[id].Set(entity.EntityID, -1, (short)(entity.Version + 1), 0);
            _freeEntityIndexStack.Push(id);
        }
        public bool IsActive(in Entity entity)
        {
            int id = (int)entity.EntityID - 1;
            if (id < 0 || id >= TotalEntityCount)
            {
                Debug.LogWarning($"EntityManager IsActive Error:Index:{id} Out Of Range [1,{TotalEntityCount})");
                return false;
            }
            return _indexInActiveEntityIndexStack[id] !=-1;
        }
        public void OnDestroy()
        {
            Array.Clear(_entities, 0, _entities.Length);
            _freeEntityIndexStack.Clear();
            _activeEntityIndexStack.Clear();
            _indexInActiveEntityIndexStack.Clear();
            _entities = null;
            _freeEntityIndexStack = null;
            _activeEntityIndexStack = null;
            _indexInActiveEntityIndexStack = null;
        }
        #region Internal Mutation Helpers
        internal void AddComponent(uint entityID, uint mask) => _entities[(int)entityID-1].AddComponent(mask);
        internal void RemoveComponent(uint entityID, uint mask) => _entities[(int)entityID-1].RemoveComponent(mask);
        internal Entity GetEntityCopy(uint entityID) => _entities[entityID - 1];
        #endregion
    }
}