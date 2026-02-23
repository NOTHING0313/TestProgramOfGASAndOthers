using System;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    public sealed class ComponentPool
    {
        public const int DEFAULT_COMPONENT_COUNT = 64;
        private ComponentTypeEnum _componentType;
        private List<Component> _components;
        private Stack<int> _freeComponentsIndexStack; 
        private List<int> _activeComponentsIndexStack;
        private int _activeCount;
        private List<int> _indexInactiveComponentsIndexStack;
        private int _totalComponentCount = 0;
        private Component _componentTemplate;
        public int ActiveComponentCount => _activeCount;
        public int FreeComponentCount => _freeComponentsIndexStack.Count;
        public int TotalComponentCount => _totalComponentCount;
        public void Init(ComponentTypeEnum componentType)
        {
            _componentType = componentType;
            _componentTemplate = (Component)Activator.CreateInstance(ComponentTypeExtension.COMPONENT_TYPE_MAPPING[componentType.GetIndex()]);
            _freeComponentsIndexStack = new();
            _components = new(DEFAULT_COMPONENT_COUNT);
            _activeComponentsIndexStack = new(DEFAULT_COMPONENT_COUNT);
            _indexInactiveComponentsIndexStack = new(DEFAULT_COMPONENT_COUNT);
            _totalComponentCount += DEFAULT_COMPONENT_COUNT;
            _components.Add(_componentTemplate.SetComponentID(0));
            for(int i = 0; i < DEFAULT_COMPONENT_COUNT; i++)
            {
                if (i != 0)
                {
                    _components.Add(_componentTemplate.GetNewInstance().SetComponentID((uint)i));
                    _freeComponentsIndexStack.Push(i);
                }
                _indexInactiveComponentsIndexStack.Add(-1);
            }
        }
        public void ExpandPool()
        {
            _components.Capacity += DEFAULT_COMPONENT_COUNT;
            _activeComponentsIndexStack.Capacity += DEFAULT_COMPONENT_COUNT;
            _indexInactiveComponentsIndexStack.Capacity += DEFAULT_COMPONENT_COUNT;
            for(int i= 0; i < DEFAULT_COMPONENT_COUNT; i++)
            {
                _components.Add(_componentTemplate.GetNewInstance().SetComponentID((uint)_totalComponentCount));
                _freeComponentsIndexStack.Push(_totalComponentCount++);
                _indexInactiveComponentsIndexStack.Add(-1);
            }
        }
        public Component GetInstance(World world, Entity entity, out uint index)
        {
            if (_freeComponentsIndexStack.Count == 0)
                ExpandPool();
            int tempIndex = _freeComponentsIndexStack.Pop();
            index = (uint)tempIndex;
            if (_activeComponentsIndexStack.Count > _activeCount)
                _activeComponentsIndexStack[_activeCount] = tempIndex;
            else
                _activeComponentsIndexStack.Add(tempIndex);
            _indexInactiveComponentsIndexStack[tempIndex] = _activeCount++;
            var temp = _components[tempIndex];
            temp.OnAttach(world, entity);
            return temp;
        }
        public void ReleaseInstance(World world, Component component, Entity entity)
        {
            if (component == null)
            {
                Debug.LogError($"ComponentPool ReleaseInstance Error:Cant Find Component)");
                return;
            }
            int index = (int)component.ComponentID;
            if (index == 0 || index >= TotalComponentCount)
            {
                Debug.LogError($"ComponentPool ReleaseInstance Error:Index:{index} Out Of Range [1,{TotalComponentCount})");
                return;
            }
            if (component.ComponentType != _componentType)
            {
                Debug.LogError($"ComponentPool ReleaseInstance Error:Wrong Type,Expect{_componentType},Got{component.ComponentType}");
                return;
            }
            int pos = _indexInactiveComponentsIndexStack[index];
            if (pos < 0)
            {
                Debug.LogError($"ComponentPool ReleaseInstance Error:Component Is Not Active");
                return;
            }
            int activePos = _activeComponentsIndexStack[--_activeCount]; 
            _indexInactiveComponentsIndexStack[activePos] = pos; 
            _activeComponentsIndexStack[pos] = activePos; 
            _indexInactiveComponentsIndexStack[index] = -1;
            component.Reset(world, entity);
            _freeComponentsIndexStack.Push(index);
        }
        public Component GetActiveInstance(uint index)
        {
            int tempIndex = (int)index;
            if (tempIndex == 0 || tempIndex >= TotalComponentCount)
            {
                Debug.LogError($"ComponentPool GetActiveInstance Error:Index:{index} Out Of Range [1,{TotalComponentCount})");
                return null;
            }
            int pos = _indexInactiveComponentsIndexStack[tempIndex];
            if (pos < 0)
            {
                Debug.LogError($"ComponentPool GetActiveInstance Error:Component Is Not Active");
                return null;
            }
            return _components[tempIndex];
        }
        public int GetAllActiveComponents(in List<Component> components)
        {
            components.Clear();
            components.Capacity = Mathf.Max(components.Capacity, _activeCount);
            for (int i = 0; i < _activeCount; i++)
                components.Add(_components[_activeComponentsIndexStack[i]]);
            return ActiveComponentCount;
        }
        public void OnDestroy()
        {
            foreach (var temp in _components)
                temp.OnDestroy();
            if (_componentTemplate != null && (_components == null || _components.Count == 0 || !ReferenceEquals(_components[0], _componentTemplate)))
                _componentTemplate.OnDestroy();
            _components.Clear();
            _components = null;
            _componentTemplate = null;
            _freeComponentsIndexStack.Clear();
            _freeComponentsIndexStack = null;
            _activeComponentsIndexStack.Clear();
            _activeComponentsIndexStack = null;
            _indexInactiveComponentsIndexStack.Clear();
            _indexInactiveComponentsIndexStack = null;
        }
    }
}
