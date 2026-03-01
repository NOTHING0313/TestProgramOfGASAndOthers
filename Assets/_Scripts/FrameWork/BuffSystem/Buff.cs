using Utility.EventCente;
namespace BuffSystem
{
    public abstract class Buff 
    {
        public BuffConfigData ConfigData;
        public BuffRuntimeData RunTimeData;
        /// <summary>
        /// Buff是否应该在下一帧结束
        /// </summary>
        public virtual bool IsCompletelyOver => !ConfigData.IsForever && RunTimeData.Stack <= 0;
        /// <summary>
        /// 指示Buff本帧是否应该降层
        /// </summary>
        public virtual bool IsStackOver => !ConfigData.IsForever && RunTimeData.RunTime >= RunTimeData.ActualDuration;
        public abstract void StartBuff();
        public abstract void EndBuff();
        public abstract void IncreaseBuffStack(int buffStackCount = 1);
        public abstract void DecreaseBuffStack(int buffStackCount = 1);
        public abstract void TriggerBuff(IEventData eventData);
    }
}
