using UnityEngine;
using System;
using BuffSystem;
using GAS;
namespace PoolSystem
{
public static class ReferenceTypes
{
public const int REFERENCE_TYPE_COUNT = 12;
public const uint GAMEOBJECTPOOL = 0;
public const uint BUFFRUNTIMEDATA = 1;
public const uint PARALLELBUFFRUNTIMEDATA = 2;
public const uint ABILITYRUNTIMECONTEXT = 3;
public const uint BLACKBOARD = 4;
public const uint ABILITYCOMPONENT = 5;
public const uint ABILITYCOMPONENTCONTEXT = 6;
public const uint ATTRIBUTESET = 7;
public const uint ATTRIBUTE = 8;
public const uint ATTRIBUTESETSNAPSHOT = 9;
public const uint ATTRIBUTESNAPSHOT = 10;
public const uint BLACKBOARDSNAPSHOT = 11;
public static readonly Type[] types = new Type[REFERENCE_TYPE_COUNT]
{typeof(GameObjectPool),typeof(BuffRuntimeData),typeof(ParallelBuffRunTimeData),typeof(AbilityRuntimeContext),typeof(BlackBoard),typeof(AbilityComponent),typeof(AbilityComponentContext),typeof(AttributeSet),typeof(GAS.Attribute),typeof(AttributeSet.AttributeSetSnapShot),typeof(GAS.Attribute.AttributeSnapShot),typeof(BlackBoard.BlackBoardSnapShot)};
        /// <summary>
        /// 获取类型对应的反射
        /// </summary>
        /// <param name="referenceType">
        /// 类型
        /// </param>
        /// <param name="reference">
        /// 对应的反射类型
        /// </param>
        /// <returns>
        /// 是否成功获取
        /// </returns>
        public static bool GetReference(in uint referenceType, out Type reference)
        {
            if (referenceType > REFERENCE_TYPE_COUNT)
            {
                Debug.LogWarning($"ReferenceType GetReference Warning:{referenceType} Is Out Of Range");
                reference = null;
                return false;
            }
            reference = types[referenceType];
            return true;
        }
        /// <summary>
        /// 获取类型在列表中的位置
        /// </summary>
        /// <typeparam name="TReference"></typeparam>
        /// <returns></returns>
        public static int GetReferenceTypeIndex<TReference>() where TReference : IReference<TReference>, new()
        {
            return GetReferenceTypeIndex(typeof(TReference));
        }
        /// <summary>
        /// 获取类型在列表中的位置
        /// </summary>
        /// <returns></returns>
        public static int GetReferenceTypeIndex(Type referenceType)
        {
            for (int i = 0; i < REFERENCE_TYPE_COUNT; i++)
            {
                if (types[i] == referenceType)
                    return i;
            }
            return -1;
        }
    }
}