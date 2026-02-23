using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GAS
{
    /// <summary>
    /// 技能行为编写的最小单位
    /// </summary>
    public abstract class AbilityBehaviourUnit : ScriptableObject
    {
        [field: SerializeField] public HeadInfo HeadInfo { get; set; }//[field:SerializeField]用于标注public的自动属性，来使其能够在insepector上面显示
        public int RuntimeToken { get; private set; }

    }
}
