using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine.Rendering.VirtualTexturing;

namespace ECS
{
    public class World
    {
        private GameObjectRegistration _registration;
        private ComponentPoolManager _componentPoolManager;
        private EntityManager _entityManager;
        private SparseArray[] _entitySearchSparseArrays;
        private SparseArray[] _componentSearchSparseArrays;
        private List<Query> _activeQuriesCurrentFrame;
        private List<ISystem> _systems;

        public int GetActiveComponentCount(ComponentTypeEnum type) => _componentPoolManager.GetComponentPool(type).ActiveComponentCount;
    }
}