using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    public readonly struct EntityHandle : IEquatable<EntityHandle>
    {
        public readonly uint ID;
        public readonly ushort Version;
        public EntityHandle(uint id, ushort version) => (ID, Version) = (id, version);
        public bool IsValid => ID != 0;
        public bool Equals(EntityHandle other) => ID.Equals(other.ID) && Version.Equals(other.Version);
        public override bool Equals(object obj) => obj is EntityHandle other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(ID, Version);
        public static bool operator ==(EntityHandle a, EntityHandle b) => a.Equals(b);
        public static bool operator !=(EntityHandle a, EntityHandle b) => !(a == b);
        public override string ToString() => $"Entity({ID},{Version})";
    }
}
