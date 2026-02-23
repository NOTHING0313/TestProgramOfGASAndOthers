using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public sealed class AudioSystemTest : MonoBehaviour
    {
        public GameObject GameObjectTarget;   
        Queue<AudioHandle> Test1 = new();
        Queue<AudioHandle> Test2 = new();
        Queue<AudioHandle> Test3 = new();
        [HorizontalGroup("AudioSystem²âÊÔ")]
        [VerticalGroup("AudioSystem²âÊÔ/²¥·Å"), Button("²¥·Å²âÊÔÒôÆµ1")]
        public void Play1() => Test1.Enqueue(AudioManager.Instance.Play("au_test001"));
        [VerticalGroup("AudioSystem²âÊÔ/²¥·Å"), Button("²¥·Å²âÊÔÒôÆµ2")]
        public void Play2() => Test2.Enqueue(AudioManager.Instance.Play("au_test002"));
        [VerticalGroup("AudioSystem²âÊÔ/²¥·Å"), Button("²¥·Å²âÊÔÒôÆµ3")]
        public void Play3() => Test3.Enqueue(AudioManager.Instance.PlayFollow("au_test003",GameObjectTarget.transform));
        [Button("ÔÝÍ£²¥·Å²âÊÔÒôÆµ2")]
        public void Pause2(){ AudioHandle temp = Test2.Dequeue(); AudioManager.Instance.Pause(temp); Test2.Enqueue(temp); }
        [Button("¼ÌÐø²¥·Å²âÊÔÒôÆµ2")]
        public void Resume2() { AudioHandle temp = Test2.Dequeue(); AudioManager.Instance.Resume(temp); Test2.Enqueue(temp); }
        [VerticalGroup("AudioSystem²âÊÔ/½áÊø"), Button("½áÊø²âÊÔÒôÆµ1")]
        public void Stop1() => AudioManager.Instance.Stop(Test1.Count > 0 ? Test1.Dequeue() : default);
        [VerticalGroup("AudioSystem²âÊÔ/½áÊø"), Button("½áÊø²âÊÔÒôÆµ2")]
        public void Stop2() => AudioManager.Instance.Stop(Test2.Count > 0 ? Test2.Dequeue() : default);
        [VerticalGroup("AudioSystem²âÊÔ/½áÊø"), Button("½áÊø²âÊÔÒôÆµ3")]
        public void Stop3() => AudioManager.Instance.Stop(Test3.Count > 0 ? Test3.Dequeue() : default);
    }
}
