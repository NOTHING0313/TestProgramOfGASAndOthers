using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    /// <summary>
    /// 一个Ability的配置文件，用作AbilityEffect及其触发器的容器
    /// </summary>
    public class Ability
    {
        [field: SerializeField] public HeadInfo HeadInfo { get; private set; }
        [field: SerializeField] public AbilityTriggerUnit TriggerUnit { get; private set; }
        [field: SerializeField]public List<AbilityEffect> Effects { get; private set; }
        public void OnBuild(HeadInfo headInfo,AbilityTriggerUnit triggerUnit,List<AbilityEffect> effects)
        {
            HeadInfo = headInfo;
            TriggerUnit = triggerUnit;
            Effects = effects;
        }
    }
}
