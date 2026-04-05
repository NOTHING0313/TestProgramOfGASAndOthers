
using System.Collections.Generic;

namespace ECS
{
    public class EntityManager 
    {
        private List<EntityMeta> _metas;
        private Stack<uint> _freeMetas;
        private int _nextVersion = 0;
        private const int DEFUALT_COUNT = 16;
        public int EntityCount => _metas.Count;
        public int FreeEntityCount => _freeMetas.Count;
        private void Expand()
        {
            if (_metas == null)
                _metas = new(DEFUALT_COUNT);
            else
                _metas.Capacity += DEFUALT_COUNT;
            _freeMetas ??= new();
            for(int i = 0; i < DEFUALT_COUNT; i++)
            {

            }
        }
    }
}
