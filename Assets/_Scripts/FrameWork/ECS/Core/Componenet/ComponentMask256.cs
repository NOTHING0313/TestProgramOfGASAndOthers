using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Utility.BitSet;
namespace ECS
{
    public struct ComponentMask256 : IEquatable<ComponentMask256>
    {
        private ulong _w0;
        private ulong _w1;
        private ulong _w2;
        private ulong _w3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]//方法小且调用频繁，因此尽量直接展开到调用的地方
        public void Set(ComponentTypeID id)
        {
            switch (id.WordIndex)
            {
                case 0: _w0 |= id.WordMask; break;
                case 1: _w1 |= id.WordMask; break;
                case 2: _w2 |= id.WordMask; break;
                case 3: _w3 |= id.WordMask; break;
                default: throw new ArgumentOutOfRangeException(nameof(id), "ComponentMask256 Set Error:ComponentTypeId Exceeds 256 Bits.");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]//方法小且调用频繁，因此尽量直接展开到调用的地方
        public void Remove(ComponentTypeID id)
        {
            ulong inv = ~id.WordMask;
            switch (id.WordIndex)
            {
                case 0: _w0 &= inv; break;
                case 1: _w1 &= inv; break;
                case 2: _w2 &= inv; break;
                case 3: _w3 &= inv; break;
                default: throw new ArgumentOutOfRangeException(nameof(id), "ComponentMask256 Set Error:ComponentTypeId Exceeds 256 Bits.");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(ComponentTypeID id)
        {
            return id.WordIndex switch
            {
                0 => (_w0 & id.WordMask) != 0,
                1 => (_w1 & id.WordMask) != 0,
                2 => (_w2 & id.WordMask) != 0,
                3 => (_w3 & id.WordMask) != 0,
                _ => false
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsAll(in ComponentMask256 other) => (_w0 & other._w0) == other._w0 && (_w1 & other._w1) == other._w1 && (_w2 & other._w2) == other._w2 && (_w3 & other._w3) == other._w3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsAny(in ComponentMask256 other) => ((_w0 & other._w0) | (_w1 & other._w1) | (_w2 & other._w2) | (_w3 & other._w3)) != 0;
        public bool IsEmpty() => (_w0 | _w1 | _w2 | _w3) == 0;
        public int PopCount() => BitOperations.PopCount(_w0) + BitOperations.PopCount(_w1) + BitOperations.PopCount(_w2) + BitOperations.PopCount(_w3);
        public bool Equals(ComponentMask256 other) => _w0 == other._w0 && _w1 == other._w1 && _w2 == other._w2 && _w3 == other._w3;
        public override bool Equals(object obj) => obj is ComponentMask256 other && Equals(other);
        public override string ToString() => $"[{Convert.ToString((long)_w3, 2)}|{Convert.ToString((long)_w2, 2)}|{Convert.ToString((long)_w1, 2)}|{Convert.ToString((long)_w0, 2)}]";//将_w0…转换成2进制输出
        public override int GetHashCode() => HashCode.Combine(_w0, _w1, _w2, _w3);
        public static bool operator ==(ComponentMask256 a, ComponentMask256 b) => a.Equals(b);
        public static bool operator !=(ComponentMask256 a, ComponentMask256 b) => !(a == b);
        public void ClearAll() => _w0 = _w1 = _w2 = _w3 = 0;

    }
}
