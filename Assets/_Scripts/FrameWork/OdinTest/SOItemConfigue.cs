using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
namespace OdinLearningTest
{
    [CreateAssetMenu(fileName = "SO1", menuName = "SO")]
    public class SOItemConfigue : ScriptableObject
    {
        [LabelText("名字"), BoxGroup("基础信息")] public string rename;
        [LabelText("性别"), BoxGroup("基础信息")] public string sex;
        [LabelText("朋友"), BoxGroup("基础信息"), SerializeField] public List<SOSingle> friend = new();
    }
}