using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BuffSystem
{
    [CreateAssetMenu(menuName = "BuffSystem/BuffConfigData", fileName = "BuffConfigData")]
    public class BuffConfigData : ScriptableObject
    {
        [BoxGroup("基础信息"), LabelText("ID")]
        public int ID;
        [BoxGroup("基础信息"), LabelText("名称")]
        public string Name;
        [BoxGroup("基础信息"), LabelText("描述")]
        public string Description;
        [BoxGroup("基础信息"), LabelText("图标")]
        public Sprite Icon;
        [BoxGroup("基础信息"), LabelText("优先级"), Tooltip("数值越低，优先级越高")]
        public int Priority;
        [BoxGroup("基础信息"), LabelText("Tag")]
        public List<BuffTag> Tags;

        [BoxGroup("生命周期配置"), LabelText("无层数上限")]
        public bool Unlimited = false;
        [BoxGroup("生命周期配置"), LabelText("最大层数"), MinValue(0), HideIf("@Unlimited")]
        public int MaxStack = 1;
        [BoxGroup("生命周期配置"), LabelText("永久Buff")]
        public bool IsForever = false;
        [BoxGroup("生命周期配置"), LabelText("持续时间"), MinValue(0), HideIf("@IsForever")]
        public float Duration = 1;
        [BoxGroup("生命周期配置"), LabelText("Buff触发类型")]
        public BuffTriggerType BuffTriggerType;
        private bool IsTick => BuffTriggerType == BuffTriggerType.Tick;
        [BoxGroup("生命周期配置"), LabelText("周期型Buff的触发周期"), ShowIf("@IsTick"), InfoBox("当前触发周期大于buff持续时间", infoMessageType: InfoMessageType.Warning, VisibleIf = "@TickTime>Duration")]
        public float TickTime = 0;
        [BoxGroup("生命周期配置"), LabelText("是否使用预制的buff层数处理逻辑")]
        public bool UseDefualtStackStrategy;
        [BoxGroup("生命周期配置"), LabelText("Buff层数增加方案")]
        public BuffStackUpStrategy BuffStackUpStrategy;
        [BoxGroup("生命周期配置"), LabelText("Buff层数减少方案")]
        public BuffStackDownStrategy BuffStackDownStrategy;
        [BoxGroup("生命周期配置"), LabelText("每层增加时间"), Tooltip("当层数影响Buff持续时间时,每一层增加多少时间")]
        public float DurationExtendPerStack;

        [BoxGroup("Buff效果配置"), LabelText("Buff效果")]
        public BuffEffect BuffEffect;
        /// <summary>
        /// 拷贝数据，防止污染配置文件
        /// </summary>
        /// <param name="emptyData"></param>
        public virtual void CopyTo(BuffConfigData emptyData)
        {
            if (emptyData == null)
            {
                Debug.LogError("BuffConfigData CopyTo Error:EmptyData Is Null");
                return;
            }
            emptyData.ID = ID;
            emptyData.Name = Name;
            emptyData.Description = Description;
            emptyData.Icon = Icon;
            emptyData.Priority = Priority;
            emptyData.Tags = Tags;
            emptyData.MaxStack = MaxStack;
            emptyData.IsForever = IsForever;
            emptyData.Duration = Duration;
            emptyData.TickTime = TickTime;
            emptyData.UseDefualtStackStrategy = UseDefualtStackStrategy;
            emptyData.BuffStackUpStrategy = BuffStackUpStrategy;
            emptyData.BuffStackDownStrategy = BuffStackDownStrategy;
            emptyData.DurationExtendPerStack = DurationExtendPerStack;
            emptyData.BuffEffect = BuffEffect;
        }
    }
}
