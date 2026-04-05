using System;
namespace ECS
{
    public readonly struct ComponentTypeID : IEquatable<ComponentTypeID>
    {
        /// <summary>
        /// ¿‡–ÕID
        /// </summary>
        public readonly ushort Value;
        public readonly byte WordIndex;
        public readonly ulong WordMask;
        public ComponentTypeID(ushort value)
        {
            Value = value;
            WordIndex = (byte)(value >> 6);
            WordMask = 1UL << (value & 63);
        }
        public bool Equals(ComponentTypeID other) => Value.Equals(other);
        public override bool Equals(object obj) => obj is ComponentTypeID other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Value);
        public override string ToString() => $"TypeID{Value}";
        public static bool operator ==(ComponentTypeID a, ComponentTypeID b) => a.Equals(b);
        public static bool operator !=(ComponentTypeID a, ComponentTypeID b) => !(a == b);
    }
}
