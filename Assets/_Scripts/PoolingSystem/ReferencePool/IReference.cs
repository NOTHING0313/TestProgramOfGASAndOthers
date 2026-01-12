using System;

namespace PoolingSystem.ReferencePool
{
    public interface IReference:IDisposable
    {
        public uint ReferenceType { get; }
        internal int IndexInReferencePool { get; set; }
        public void OnRecycle();
        public IReference GetNewInstance();
    }
    public interface IReference<TReference> : IReference where TReference : IReference, new() { }
}