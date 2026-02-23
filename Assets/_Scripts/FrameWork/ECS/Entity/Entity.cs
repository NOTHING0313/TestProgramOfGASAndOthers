namespace ECS
{
    public struct Entity
    {
        public uint EntityID { get; private set; }
        public int GameObjectID { get; private set; }
        public short Version { get; private set; }
        public uint ArcheType { get; private set; }//原型,指拥有同一组组件的实体集合,本质上就是组件集合的位掩码
        public readonly bool HasComponent(ComponentTypeEnum componentType)
        {
            uint mask = componentType.ToMask();
            return (ArcheType & mask) == mask;
        }
        public readonly bool HasComponent(uint componentType) => (ArcheType & componentType) == componentType;
        public readonly bool HasAllComponent(ComponentTypeEnum componentType) => HasComponent(componentType);
        public readonly bool HasAllComponent(uint componentType) => (ArcheType & componentType) == componentType;
        public readonly bool HasAnyComponent(ComponentTypeEnum componentType) => (ArcheType & componentType.ToMask()) != 0;
        public readonly bool HasAnyComponent(uint componentType) => (ArcheType & componentType) != 0;
        public readonly bool WithoutComponent(ComponentTypeEnum componentType) => (ArcheType & componentType.ToMask()) == 0;
        public readonly bool WithoutComponent(uint componentType) => (ArcheType & componentType) == 0;
        public readonly bool WithoutAnyComponent(ComponentTypeEnum componentType) => (ArcheType & componentType.ToMask()) == 0;
        public readonly bool WithoutAnyComponent(uint componentType) => (ArcheType & componentType) == 0;
        public readonly bool WithoutAllComponent(ComponentTypeEnum componentType)
        {
            uint mask = componentType.ToMask();
            return (ArcheType & mask) != mask;
        }
        public readonly bool WithoutAllComponent(uint componentType) => (ArcheType & componentType) != componentType;
        internal void AddComponent(ComponentTypeEnum componentType) => ArcheType |= componentType.ToMask();
        internal void AddComponent(uint componentType) => ArcheType |= componentType;
        internal void RemoveComponent(ComponentTypeEnum componentType) => ArcheType &= ~componentType.ToMask();
        internal void RemoveComponent(uint componentType) => ArcheType &= ~componentType;
        public void Set(uint entityID, int gameObjectID, short version, uint archeType)
        {
            EntityID = entityID;
            GameObjectID = gameObjectID;
            Version = version;
            ArcheType = archeType;
        }
        public Entity(uint entityID, int gameObjectID, short version, uint archeType) 
        {
            this = default;
            Set(entityID, gameObjectID, version, archeType); 
        }
        public void SetArchetype(uint archeType)=> ArcheType = archeType;
        public static bool operator ==(Entity a, Entity b) => a.EntityID == b.EntityID && a.Version == b.Version;
        public static bool operator !=(Entity a, Entity b) => !(a==b);
        public override bool Equals(object obj) => obj is Entity && (Entity)obj == this;
        public override int GetHashCode() => (int)(EntityID << 16) | (ushort)Version;
    }
}
