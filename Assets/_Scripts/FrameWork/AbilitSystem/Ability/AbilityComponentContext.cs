using PoolSystem;
using System.Collections.Generic;

namespace GAS
{
    public class AbilityComponentContext : IReference<AbilityComponentContext>
    {
        public IReadOnlyDictionary<int,Ability> Abilities { get; private set; }
        public Dictionary<ControllerTypeEnum,IController> Controllers { get; private set; }
        public BlackBoard GlobalBlackBoard { get; private set; }
        public AttributeSet AttributeSet { get; private set; }
        public void LoadAbilitiesConfig(Dictionary<int,Ability> abilities)
        {
            Abilities = abilities;
            GlobalBlackBoard = ReferencePoolCenter.Instance.GetReference<BlackBoard>();
        }
        public void Bind(AttributeSet attributeSet)
        {
            AttributeSet = attributeSet;
        }
        public void RegisterController(ControllerTypeEnum controllerType, IController controller)
        {
            if (Controllers == null)
                Controllers = new();
            Controllers[controllerType]=controller;
        }
        #region IReference
        int IReference.IndexInReferencePool { get; set; }
        public IReference GetNewInstance() => new AbilityComponentContext();
        public void OnRecycle()
        {
            ReferencePoolCenter.Instance.ReleaseReference(GlobalBlackBoard);
            GlobalBlackBoard = null;
            AttributeSet = null;
            Controllers.Clear();
            Controllers = null;
            Abilities = null;
        }
        public void Dispose() => OnRecycle();
        #endregion
    }
}