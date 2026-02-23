using PoolSystem;
using System;
using UnityEngine;

namespace ECS
{
    public class ComponentSet : IReference<ComponentSet>
    {
        private Component[] _components = new Component[ComponentTypeExtension.COMPONENT_TYPE_COUNT];
        public Component GetComponent(ComponentTypeEnum componentType)
        {
            uint index = componentType.GetIndex();
            if (_components[index] != null)
                return _components[index];
            return null;
        }
        public TComponent GetComponent<TComponent>(ComponentTypeEnum componentType) where TComponent : Component => GetComponent(componentType) as TComponent;
        public void AddComponent(ComponentTypeEnum componentType,Component component)
        {
            uint index = componentType.GetIndex();
            if (_components[index] != null)
            {
                Debug.LogError($"ComponentSet Error:ComponentSet Already Contains Component of Type {componentType}");
                return;
            }
            _components[index] = component;
        }
        #region IReference
        public uint ReferenceType => ReferenceTypes.COMPONENTSET;
        int IReference.IndexInReferencePool { get; set; }
        public IReference GetNewInstance() => new ComponentSet();
        public void OnRecycle() => Array.Clear(_components, 0, _components.Length);
        public void Dispose()
        {
            OnRecycle();
            _components = null;
        }
        #endregion
    }
}
