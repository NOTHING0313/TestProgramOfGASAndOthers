using PoolSystem;
using RollBackSystem;
using System;
using UnityEngine;
namespace GAS
{
    /// <summary>
    /// 角色/单位的数值属性
    /// </summary>
    [Serializable]
    public class Attribute : IReference<Attribute>,IRollBackable
    {
        private AttributeData _attributeData;
        public float BaseValue => _attributeData.BaseValue;
        public float MaxValue => _attributeData.MaxValue;
        public float MinValue => _attributeData.MinValue;
        public event Action<float, float> OnValueChanged;
        public event Action<float, float> OnMaxValueChanged;
        public event Action<float, float> OnMinValueChanged;
        public Attribute() { }
        public Attribute(AttributeData data)
        {
            _attributeData = data;
        }
        public int Int() => (int)BaseValue;
        public float Float() => BaseValue;
        public bool Bool() => !Mathf.Approximately(BaseValue, 0f);
        public void SetBaseValue(float newValue,bool isInvokeEvent = true)
        {
            newValue = Mathf.Clamp(newValue, MinValue, MaxValue);
            if (!Mathf.Approximately(newValue, BaseValue))
            {
                float oldValue = BaseValue;
                _attributeData.BaseValue = newValue;
                if (isInvokeEvent)
                    OnValueChanged?.Invoke(oldValue, BaseValue);
            }
        }
        public void SetMaxValue(float newMaxValue,bool isInvokeEvent = true)
        {
            if (newMaxValue < MinValue)
            {
                Debug.LogWarning($"Attribute Warning:MaxValue Must Bigger Than MinValue,But {newMaxValue}<{MinValue}");
                return;
            }
            if (!Mathf.Approximately(newMaxValue, MaxValue))
            {
                float oldMaxValue = MaxValue;
                _attributeData.MaxValue = newMaxValue;
                if (BaseValue > MaxValue)
                {
                    float oldValue = BaseValue;
                    _attributeData.BaseValue = MaxValue;
                    if (isInvokeEvent)
                        OnValueChanged?.Invoke(oldValue, BaseValue);
                }
                if (isInvokeEvent)
                    OnMaxValueChanged?.Invoke(oldMaxValue, MaxValue);
            }
        }
        public void SetMinValue(float newMinValue, bool isInvokeEvent = true)
        {
            if (newMinValue > MaxValue)
            {
                Debug.LogWarning($"Attribute Warning:MinValue Must Smaller Than MaxValue,But {newMinValue}>{MaxValue}");
                return;
            }
            if (!Mathf.Approximately(newMinValue, MinValue))
            {
                float oldMinValue = MinValue;
                _attributeData.MinValue = newMinValue;
                if (BaseValue < MinValue)
                {
                    float oldValue = BaseValue;
                    _attributeData.BaseValue = MinValue;
                    if (isInvokeEvent)
                        OnValueChanged?.Invoke(oldValue, BaseValue);
                }
                if (isInvokeEvent)
                    OnMinValueChanged?.Invoke(oldMinValue, MinValue);
            }
        }

        #region IReference
        public IReference GetNewInstance() => new Attribute(new AttributeData());
        int IReference.IndexInReferencePool { get; set; }
        public void OnRecycle()
        {
            _attributeData.BaseValue = 0;
            _attributeData.MaxValue = 0;
            _attributeData.MinValue = 0;
            OnValueChanged = null;
            OnMaxValueChanged = null;
            OnMinValueChanged = null;
        }
        public void Dispose()=> OnRecycle();
        #endregion
        #region IRollBack
        internal class AttributeSnapShot : ISnapShot,IReference<AttributeSnapShot>
        {
            public AttributeData AttributeDataCopy;
            public int LocalizedLogicFrameCount { get; set; }
            public void Release() => ReferencePoolCenter.Instance.ReleaseReference(this);
            int IReference.IndexInReferencePool { get; set; }
            public IReference GetNewInstance() => new AttributeSnapShot();
            public void OnRecycle() { }
            public void Dispose() => OnRecycle();
        }
        public ISnapShot SnapShot(int localizedLogicFrameCount)
        {
            AttributeSnapShot attributeSnapShot = ReferencePoolCenter.Instance.GetReference<AttributeSnapShot>();
            attributeSnapShot.LocalizedLogicFrameCount = localizedLogicFrameCount;
            attributeSnapShot.AttributeDataCopy = _attributeData;
            return attributeSnapShot;
        }
        public void RollBack(ISnapShot attributeSnapShot,int errorLocalizedLogicFrameCount, int currentLocalizedLogicFrameCount)
        {
            var temp = attributeSnapShot as AttributeSnapShot;
            if (temp == null)
            {
                Debug.LogError("Attribute RollBack Error:Invalid SnapShot Type");
                return;
            }
            _attributeData = temp.AttributeDataCopy;
        }
        #endregion
    }
}
