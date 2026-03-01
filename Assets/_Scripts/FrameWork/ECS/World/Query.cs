using PoolSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    public class Query : IReference<Query>
    {
        private World _world;
        private List<Entity> _entities;
        private List<ComponentSet> _componentSets;
        private ComponentTypeEnum[] _componentTypes;
        private uint _includeMasks;
        private uint _excludeMasks;
        public IReadOnlyList<ComponentSet> ComponentSets => _componentSets;
        public IReadOnlyList<Entity> Entities => _entities;
        public Query With(ComponentTypeEnum componentType)
        {
            _includeMasks |= componentType.ToMask();
            return this;
        }
        public Query Without(ComponentTypeEnum componentType)
        {
            _excludeMasks |= componentType.ToMask();
            return this;
        }
        /// <summary>
        /// ÷¥––≤È—Ø
        /// </summary>
        /// <returns></returns>
        //public Query Execute()
        //{
        //    ComponentTypeEnum[] componentTypes = _includeMasks.MaskToEnum();
        //    if (componentTypes.Length == 0)
        //        return this;
        //    if (_world == null)
        //    {
        //        Debug.LogError("Query Execute Error:World Is Null");
        //        return this;
        //    }
        //    ComponentTypeEnum pivotType = componentTypes[0];
        //    int minCount = _world.GetActiveComponentCount(componentTypes[0]);
        //    for (int i = 1; i < componentTypes.Length; i++)
        //    {
        //        int tempCount = _world.GetActiveComponentCount(componentTypes[i]);
        //        if (tempCount < minCount)
        //        {
        //            minCount = tempCount;
        //            pivotType = componentTypes[i];
        //        }
        //    }
        //}
        public Query()
        {
            _componentSets = new();
            _entities = new();
            _includeMasks = 0;
            _excludeMasks = 0;
        }
        public Query BindWorld(World world)
        {
            _world = world;
            return this;
        }
        #region IReference
        public uint ReferenceType => ReferenceTypes.QUERY;
        int IReference.IndexInReferencePool { get; set; }
        public IReference GetNewInstance() => new Query();
        public void OnRecycle()
        {
            _entities.Clear();
            foreach (var set in _componentSets)
            {
                ReferencePoolCenter.Instance.ReleaseReference(set);
            }
            _componentSets.Clear();
            if (_componentTypes != null)
                Array.Clear(_componentTypes, 0, _componentTypes.Length);
            _includeMasks = 0;
            _excludeMasks = 0;
            _world = null;
        }
        public void Dispose()
        {
            OnRecycle();
            _componentSets = null;
            _entities = null;
            _componentTypes = null;
        }
        #endregion
    }
}
