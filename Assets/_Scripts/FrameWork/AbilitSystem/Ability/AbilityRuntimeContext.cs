using PoolSystem;
using RollBackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GAS
{
    public class AbilityRuntimeContext //: IReference<AbilityRuntimeContext>, IRollBackable
    {
        public bool Interuptable;
        private Dictionary<int, BlackBoard> LocalBlackBoard = new();
        public int AbilityID { get; private set; }
        public AbilityComponentContext AbilityComponentContext { get; private set; }
        public AbilityComponent AbilityComponent { get; private set; }
        public Ability Ability => AbilityComponentContext.Abilities[AbilityID];

        #region IReference
        #endregion
        #region IRollBack
        #endregion
    }
}
