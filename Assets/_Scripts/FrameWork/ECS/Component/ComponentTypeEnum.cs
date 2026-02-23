using GAS;
using System;

namespace ECS
{
    [Flags]
    public enum ComponentTypeEnum
    {
        AbilityComponent = 1,
        InputComponent = 0b10,
        RollBackComponent = 0b100,
        TagComponent = 0b1000,
        CharactorTransformControllerComponent = 0b10000,
        CharactorAnimationControllerComponent = 0b100000,
        AttributeComponent = 0b1000000,
        BulletComponent = 0b10000000
    }
    public static class ComponentTypeExtension
    {
        public const int COMPONENT_TYPE_COUNT = 8;
        public static readonly Type[] COMPONENT_TYPE_MAPPING = new Type[COMPONENT_TYPE_COUNT]{
            typeof(AbilityComponent),
            typeof(InputComponent),
            typeof(RollBackComponent),
            typeof(TagComponent),
            typeof(CharactorTransformController),
            typeof(CharactorAnimationController),
            typeof(AttributeComponent),
            typeof(BulletComponent),
        };
        public static uint GetComponentMask<TComponent>() where TComponent : Component
        {
            var type = typeof(TComponent);
            for (int i = 0; i < COMPONENT_TYPE_MAPPING.Length; i++)
                if (type == COMPONENT_TYPE_MAPPING[i])
                    return 1u << i;
            throw new ArgumentException($"ComponentTypeExtension Error:Component Type Not Registered: {type.FullName}");
        }

        public static uint ToMask(this ComponentTypeEnum componentType) => EnumToMask(componentType);
        public static uint GetIndex(this ComponentTypeEnum componentType)
        {
            int value = (int)componentType;
            if ((value & (value - 1)) != 0)
                throw new ArgumentException($"ComponentTypeExtension Error:{componentType}={value},Invalid Enum Value");//防御性编程，及时暴露，避免错误的index污染数据结构
            uint index = 0;
            while (value != 0)
            {
                value >>= 1;
                index++;
            }
            return index;
        }
        public static uint EnumToMask(params ComponentTypeEnum[] componentTypes)
        {
            if (componentTypes == null || componentTypes.Length == 0)
                return 0;
            uint mask = 0;
            foreach (var type in componentTypes)
                mask |= (uint)type;
            return mask;
        }
        public static ComponentTypeEnum[] MaskToEnum(this uint mask)
        {
            if (mask == 0)
                return Array.Empty<ComponentTypeEnum>();
            uint temp = mask;
            int count = 0;
            while (temp != 0)
            {
                temp &= (temp - 1);
                count++;
            }
            if (count > COMPONENT_TYPE_COUNT)
            {
#if UNITY_5_3_OR_NEWER
                UnityEngine.Debug.LogWarning(
                    $"ComponentTypeExtension Warning:The Bit Set Of Mask：{count} Is Bigger Than COMPONENT_TYPE_COUNT");
#endif
                count = COMPONENT_TYPE_COUNT;
            }
            ComponentTypeEnum[] ans = new ComponentTypeEnum[count];
            int index = 0;
            int value = 1;
            for (int i = 0; i < COMPONENT_TYPE_COUNT; i++)
            {
                if ((mask & value) != 0)
                    ans[index++] = (ComponentTypeEnum)value;
                value <<= 1;
            }
            return ans;
        }
    }
}
