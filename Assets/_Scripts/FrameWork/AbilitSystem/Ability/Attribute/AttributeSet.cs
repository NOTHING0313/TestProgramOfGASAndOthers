using PoolSystem;
using RollBackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
namespace GAS
{
    /// <summary>
    /// AttributeµÄ¼¯ºÏ
    /// </summary>
    public class AttributeSet:IReference<AttributeSet>,IRollBackable
    {
        public Dictionary<int, Attribute> map = new();
        public Attribute GetAttribute(int attributeID)
        {
            if (map.ContainsKey(attributeID))
                return map[attributeID];
            Debug.LogWarning($"AttriteSet Warning:Attribute ID {attributeID} Cant Found");
            return null;
        }
        public Attribute this[int id]=>GetAttribute(id);
        public void AddAttribute(int attributeID,Attribute newValue)
        {
            if (!map.ContainsKey(attributeID))
                map.Add(attributeID, newValue);
        }
        public void RemoveAttribute(int attributeID)
        {
            if (map.ContainsKey(attributeID))
                map.Remove(attributeID);
        }
        #region IReference
        public uint ReferenceType => ReferenceTypes.ATTRIBUTESET;
        int IReference.IndexInReferencePool { get; set; }
        public IReference GetNewInstance() => new AttributeSet();
        public void OnRecycle()=> map.Clear();
        public void Dispose()
        {
            OnRecycle();
            map = null;
        }
        #endregion
        #region IRollBack
        internal class AttributeSetSnapShot : IReference<AttributeSetSnapShot>,ISnapShot
        {
            public List<int> MapKeyCopy;
            public List<ISnapShot> MapValueCopy;
            public uint ReferenceType => ReferenceTypes.ATTRIBUTESETSNAPSHOT;
            public int LocalizedLogicFrameCount { get; }
            int IReference.IndexInReferencePool { get; set; }
            public IReference GetNewInstance()
            {
                AttributeSetSnapShot temp = new();
                temp.MapKeyCopy = ListPool<int>.Get();
                temp.MapValueCopy = ListPool<ISnapShot>.Get();
                return temp;
            }
            public void Release() => ReferencePoolCenter.Instance.ReleaseReference(this);
            public void OnRecycle()
            {
                MapKeyCopy.Clear();
                MapValueCopy.Clear();
            }
            public void Dispose()
            {
                OnRecycle();
                if (MapKeyCopy != null)
                    ListPool<int>.Release(MapKeyCopy);
                if (MapValueCopy != null)
                    ListPool<ISnapShot>.Release(MapValueCopy);
                MapKeyCopy = null;
                MapValueCopy = null;
            }
        }
        public ISnapShot SnapShot(int localizedLogicalFrameCount)
        {
            AttributeSetSnapShot attributeSetSnapShot = ReferencePoolCenter.Instance.GetReference<AttributeSetSnapShot>();
            if (attributeSetSnapShot.MapKeyCopy.Capacity < map.Count)
            {
                attributeSetSnapShot.MapKeyCopy.Capacity = map.Count;
                attributeSetSnapShot.MapValueCopy.Capacity = map.Count;
            }
            foreach(var pair in map)
            {
                attributeSetSnapShot.MapKeyCopy.Add(pair.Key);
                attributeSetSnapShot.MapValueCopy.Add(pair.Value.SnapShot(localizedLogicalFrameCount));
            }
            return attributeSetSnapShot;
        }
        public void RollBack(ISnapShot snapShot, int errorLocalizedLogicFrameCount, int currentLocalizedLogicFrameCount)
        {
            var attributeSetSnapShot = snapShot as AttributeSetSnapShot;
            if (attributeSetSnapShot == null)
            {
                Debug.LogError("AttributeSet RollBack Error:Invalid SnapShot Type");
                return;
            }
            for (int i = 0; i < attributeSetSnapShot.MapKeyCopy.Count; i++)
            {
                if (map.ContainsKey(attributeSetSnapShot.MapKeyCopy[i]))
                {
                    map[attributeSetSnapShot.MapKeyCopy[i]].RollBack(attributeSetSnapShot.MapValueCopy[i], errorLocalizedLogicFrameCount, currentLocalizedLogicFrameCount);
                    continue;
                }
                Debug.LogWarning($"AttributeSet RollBack Warning:Attribute ID {attributeSetSnapShot.MapKeyCopy[i]} Cant Found");
            }
        }
        #endregion
    }
}
