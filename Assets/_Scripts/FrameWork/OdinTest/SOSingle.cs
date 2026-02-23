using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace OdinLearningTest
{
    public class SOSingle : ScriptableObject
    {
        [LabelText("名字"), BoxGroup("基础信息")] public string rename;
        [LabelText("性别"), BoxGroup("基础信息")] public string sex;
        [LabelText("朋友"), BoxGroup("基础信息"), SerializeField] public List<SOItemConfigue> friend;
        [ContextMenu("删除自己")]
        public void DeleteSelf()
        {
            Undo.DestroyObjectImmediate(this);
            AssetDatabase.SaveAssets();
        }
    }
}
