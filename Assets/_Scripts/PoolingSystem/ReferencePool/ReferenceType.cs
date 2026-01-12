using System;

namespace PoolingSystem.ReferencePool
{
    public static class ReferenceTypes
    {
        public const uint BLACKBOARD = 0;
        public const int TYPE_COUNT = 1;
        private static Type[] types = new Type[TYPE_COUNT]
        {
            typeof(BlackBoard)
        };
        public static int GetRefenceTypeIndex<TReference>() where TReference : IReference, new()
        {
            return GetReferenceTypeIndex(typeof(TReference));
        }
        public static int GetReferenceTypeIndex(Type referenceType)
        {
            for (int i = 0; i < TYPE_COUNT; i++)
            {
                if (types[i] == referenceType)
                    return i;
            }
            return -1;
        }
    }
}