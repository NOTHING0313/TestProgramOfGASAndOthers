using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    /// <summary>
    /// Ï¡ÊèÊý×é
    /// </summary>
    public class SparseArray
    {
        private readonly List<uint[]> _sparseArrayBucket;
        private static uint[] _templateIndexArray;
        public void SetIndex(uint indexID, uint bindID)
        {
            int bucketIndex = (int)indexID / ComponentPool.DEFAULT_COMPONENT_COUNT;
            int indexInBucket = (int)indexID % ComponentPool.DEFAULT_COMPONENT_COUNT;
            while (bucketIndex >= _sparseArrayBucket.Count)
                _sparseArrayBucket.Add(AllocNewPage());
            _sparseArrayBucket[bucketIndex][indexInBucket] = bindID;
        }
        public uint GetIndex(uint indexID)
        {
            int bucketIndex = (int)indexID / ComponentPool.DEFAULT_COMPONENT_COUNT;
            if (bucketIndex > _sparseArrayBucket.Count)
            {
                Debug.LogWarning($"SparseArray GetIndex Warning:BucketIndex:{bucketIndex} Is Bigger Than The Count Of SparseArrayBucket:{_sparseArrayBucket.Count}");
                int count = bucketIndex - _sparseArrayBucket.Count;
                _sparseArrayBucket.Capacity += count;
                while (count-- > 0)
                    _sparseArrayBucket.Add(AllocNewPage());
            }
            int indexInBucket = (int)indexID % ComponentPool.DEFAULT_COMPONENT_COUNT;
            return _sparseArrayBucket[bucketIndex][indexInBucket];
        }
        public void RemoveIndex(uint indexID)
        {
            int bucketIndex = (int)indexID / ComponentPool.DEFAULT_COMPONENT_COUNT;
            if (bucketIndex >= _sparseArrayBucket.Count)
            {
                Debug.LogError($"SparseArray RemoveIndex Error:The Bind ID of this Index:{indexID} Has Not Been Setted");
                return;
            }
            uint indexInBucket = indexID % ComponentPool.DEFAULT_COMPONENT_COUNT;
            _sparseArrayBucket[bucketIndex][indexInBucket] = 0;
        }
        public uint[] AllocNewPage()
        {
            uint[] newPage = new uint[ComponentPool.DEFAULT_COMPONENT_COUNT];
            Array.Copy(_templateIndexArray, newPage, ComponentPool.DEFAULT_COMPONENT_COUNT);
            return newPage;
        }
        public SparseArray() => _sparseArrayBucket = new() { AllocNewPage() };
        static SparseArray() => _templateIndexArray = new uint[ComponentPool.DEFAULT_COMPONENT_COUNT];
        public void OnDestroy()
        {
            foreach (var temp in _sparseArrayBucket)
                Array.Clear(temp, 0, ComponentPool.DEFAULT_COMPONENT_COUNT);
            _sparseArrayBucket.Clear();
            Array.Clear(_templateIndexArray, 0, ComponentPool.DEFAULT_COMPONENT_COUNT);
            _templateIndexArray = null;
        }
    }
}
