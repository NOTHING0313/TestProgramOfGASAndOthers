using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
namespace BuffSystem
{
    [CreateAssetMenu(menuName = "BuffSystem/BuffRuntimeData", fileName = "BuffRuntimeData")]
    public class BuffRuntimeData :ScriptableObject
    {
        /// <summary>
        /// Buff的施加者
        /// </summary>
        public GameObject Sourse;
        /// <summary>
        /// Buff的接收者
        /// </summary>
        public GameObject Target;
        /// <summary>
        /// 实际的Buff运行时间上限
        /// </summary>
        public virtual float ActualDuration { get; set; }
        /// <summary>
        /// Buff的运行时间
        /// </summary>
        public float RunTime;
        /// <summary>
        /// Buff的下一个触发周期数
        /// </summary>
        public int Ticks;
        /// <summary>
        /// 层数
        /// </summary>
        public int Stack;
        public BuffRuntimeData(GameObject sourse, GameObject target, int stack)
        {
            Sourse = sourse;
            Target = target;
            Stack = stack;
            RunTime = 0;
            Ticks = 0;
        }
    }
}
