using PoolingSystem.ReferencePool;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


public class BlackBoard : IReference<BlackBoard>
{
    #region StructDefine
    public struct UnmanagedValueHeader
    {
        public int TypeID;
        public int Size;
        public int Align;
        public UnmanagedValueHeader(int typeID, int size, int align) { TypeID = typeID; Size = size; Align = align; }
    }
    #endregion
    #region IReference
    public uint ReferenceType => ReferenceTypes.BLACKBOARD;
    int IReference.IndexInReferencePool { get; set; }
    public void OnRecycle()
    {
        ManagedFields.Clear();
        UnmanagedFields.Clear();
        repository.Clear();
        currentRepositorySize = 0;
    }
    public void Dispose()
    {
        OnRecycle();
        //释放集合自身，防止内存泄漏，确保GC回收
        ManagedFields = null;
        UnmanagedFields = null;
        repository = null;
    }
    public IReference GetNewInstance()
    {
        return new BlackBoard();
    }
    #endregion
    #region BasicDefine
    private Dictionary<int, object> ManagedFields = new();
    private Dictionary<int, UnmanagedValueHeader> UnmanagedFields = new();
    //所有 BlackBoard 实例共享同一个 TypeIDMap 和 currentTypeID
    private static readonly Dictionary<Type, int> TypeIDMap = new();
    private List<byte> repository = new(256);
    private int currentRepositorySize = 0;
    private static int currentTypeID = 0;
    #endregion
    #region API
    public void Set<ManagedFieldType>(int id, ManagedFieldType newValue, object _ = null) where ManagedFieldType : class
    {
        if (ManagedFields.ContainsKey(id))
        {
            var oldValue = ManagedFields[id];
            if (oldValue == null || oldValue is ManagedFieldType)
                ManagedFields[id] = newValue;
            else
                Debug.LogError($"Blackboard ManagedField Set Error: Type Mismatch For ID={id}. Expected={oldValue.GetType().Name}, Got={typeof(ManagedFieldType).Name}");
        }
        else
        {
            ManagedFields.Add(id, newValue);
        }
    }
    public void Set<UnmanagedFieldType>(int id, UnmanagedFieldType newValue) where UnmanagedFieldType : unmanaged
    {
        if (UnmanagedFields.ContainsKey(id))
        {
            var headInfo = UnmanagedFields[id];
            if (headInfo.TypeID != TypeIDMap[typeof(UnmanagedFieldType)])
            {
                Debug.LogError($"Blackboard UnmanagedField Set Error: Type Mismatch For ID={id}. Expect:{headInfo.TypeID}, Got:{TypeIDMap[typeof(UnmanagedFieldType)]}");
                return;
            }
            WriteUnmanagedFields(headInfo, ref newValue);
        }
        else
        {
            UnmanagedValueHeader headInfo;
            unsafe
            {
                headInfo = new(GetTypeID<UnmanagedFieldType>(), sizeof(UnmanagedFieldType), currentRepositorySize);
            }
            currentRepositorySize += headInfo.Size;
            WriteUnmanagedFields(headInfo, ref newValue);
            UnmanagedFields.Add(id, headInfo);
        }
    }
    public ManagedFieldType Get<ManagedFieldType>(int id, object _) where ManagedFieldType : class
    {
        if (ManagedFields.ContainsKey(id))
        {
            var temp = ManagedFields[id];
            if (temp is ManagedFieldType typed)
                return typed;
            else
            {
                var got = temp == null ? "null" : temp.GetType().Name;
                Debug.LogError($"Blackboard ManagedField Get Error: Type Mismatch for ID:{id}, Expect:{typeof(ManagedFieldType).Name}, Given:{temp.GetType().Name}");
                return null;
            }
        }
        else
        {
            Debug.LogWarning($"Blackboard ManagedField Get Warning: ID Not Found,ID={id}");
            return null;
        }
    }
    public UnmanagedFieldType Get<UnmanagedFieldType>(int id)where UnmanagedFieldType : unmanaged
    {
        if (UnmanagedFields.ContainsKey(id))
        {
            UnmanagedValueHeader headInfo = UnmanagedFields[id];
            if (headInfo.TypeID != TypeIDMap[typeof(UnmanagedFieldType)])
            {
                Debug.LogError($"Blackboard UnmanagedField Get Error: Type mismatch for ID={id}. Expect:{headInfo.TypeID}, Given:{TypeIDMap[typeof(UnmanagedFieldType)]}");
                return default;
            }
            return ReadUnmanagedFields<UnmanagedFieldType>(headInfo);
        }
        else
        {
            Debug.LogWarning($"Blackboard UnmanagedField Get Warning: ID Not Found,ID={id}");
            return default;
        }
    }
    private static int GetTypeID<UnmanagedFieldType>() where UnmanagedFieldType : unmanaged
    {
        if (TypeIDMap.ContainsKey(typeof(UnmanagedFieldType)))
            return TypeIDMap[typeof(UnmanagedFieldType)];
        TypeIDMap.Add(typeof(UnmanagedFieldType), currentTypeID);
        return currentTypeID++;
    }
    private unsafe void WriteUnmanagedFields<UnmanagedFieldType>(in UnmanagedValueHeader headInfo, ref UnmanagedFieldType newValue) where UnmanagedFieldType : unmanaged
    {
        int typeSize = sizeof(UnmanagedFieldType);
        if (headInfo.Size != typeSize)
        {
            Debug.LogError($"Blackboard Write UnmanagedFields Error:headInfo.Size({headInfo.Size}) != sizeof(T)({typeSize})");
            return;
        }
        Span<byte> data = headInfo.Size <= 256 ? stackalloc byte[headInfo.Size] : new byte[headInfo.Size];
        MemoryMarshal.Write<UnmanagedFieldType>(data, ref newValue);
        for (int i = 0; i < data.Length; i++)
        {
            int tempIndex = headInfo.Align + i;
            if (tempIndex < repository.Count)
                repository[tempIndex] = data[i];
            else
            {
                int delta = tempIndex - repository.Count;
                while (delta-- > 0)
                    repository.Add(0);
                repository.Add(data[i]);
            }
        }
    }
    private unsafe UnmanagedFieldType ReadUnmanagedFields<UnmanagedFieldType>(in UnmanagedValueHeader headInfo) where UnmanagedFieldType : unmanaged
    {
        int typeSize = sizeof(UnmanagedFieldType);
        if (headInfo.Size != typeSize)
        {
            Debug.LogError($"Blackboard Read UnmanagedFields Error:headInfo.Size({headInfo.Size}) != sizeof(T)({typeSize})");
            return default;
        }
        Span<byte> data = headInfo.Size <= 256 ? stackalloc byte[headInfo.Size] : new byte[headInfo.Size];
        for (int i = 0; i < headInfo.Size; i++)
        {
            data[i] = repository[i + headInfo.Align];
        }
        return MemoryMarshal.Read<UnmanagedFieldType>(data);
    }
    #endregion
}