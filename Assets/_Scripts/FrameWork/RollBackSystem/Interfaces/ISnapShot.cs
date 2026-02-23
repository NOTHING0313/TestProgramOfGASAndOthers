using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollBackSystem
{
    public interface ISnapShot
    {
        public int LocalizedLogicFrameCount { get; }
        /// <summary>
        /// 用于嵌套快照的主动释放，外层快照调用内层快照的此方法，逻辑固定为向池化中心申请释放自己
        /// </summary>
        public void Release();
    }
}
