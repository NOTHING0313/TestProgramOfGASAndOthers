using System;

namespace ECS
{
    public sealed class ComponentPoolManager
    {
        private ComponentPool[] _pools;
        public ComponentPoolManager() => _pools = new ComponentPool[ComponentTypeExtension.COMPONENT_TYPE_COUNT];
        public ComponentPool GetComponentPool(ComponentTypeEnum componentType)
        {
            uint index = componentType.GetIndex();
            var pool = _pools[index];
            if (pool == null)
            {
                pool = new ComponentPool();
                pool.Init(componentType);
                _pools[index] = pool;
            }
            return pool;
        }
        public void OnDestroy()
        {
            if (_pools == null) 
                return;
            foreach (var pool in _pools)
                pool?.OnDestroy();
            Array.Clear(_pools, 0, _pools.Length);
            _pools = null;
        }
    }
}
