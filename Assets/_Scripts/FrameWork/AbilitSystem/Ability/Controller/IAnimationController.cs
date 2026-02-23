using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GAS
{
    public interface IAnimationController : IController
    {
        public void SetFloat(string name, float value);
        public void SetBool(string name, bool value);
        public void SetFloatSmooth(string name, float value, float smoothTime);
    }
}
