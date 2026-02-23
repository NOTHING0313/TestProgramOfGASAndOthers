using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAS
{
    /// <summary>
    /// 一个对AbilityBehaviourUnit的组，对游戏效果的打包
    /// </summary>
    public class AbilityEffect : ScriptableObject
    {
        public HeadInfo HeadInfo { get; private set; }
        public int InteruptionPriority { get; private set; }
        public AbilityBehaviourUnit RootBehaviourUnit { get; private set; }
        public void OnBuild(HeadInfo headInfo,int interuptionPriority,AbilityBehaviourUnit root)
        {
            HeadInfo = headInfo;
            InteruptionPriority = interuptionPriority;
            RootBehaviourUnit = root;
        }
    }
}
