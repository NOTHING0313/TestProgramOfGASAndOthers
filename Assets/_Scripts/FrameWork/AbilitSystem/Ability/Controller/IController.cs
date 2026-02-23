using UnityEngine;
namespace GAS
{
    /// <summary>
    /// 控制器抽象接口，所有对组件的裸操作都应该通过对应的控制器暴露的接口来实现
    /// </summary>
    public interface IController
    {
        public ControllerTypeEnum Type { get; }
        public GameObject GameObject { get; }
        public void Update();
        public void LateUpdate();
        public void LogicUpdate();
    }
}
