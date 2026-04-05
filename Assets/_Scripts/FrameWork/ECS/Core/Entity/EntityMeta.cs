using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
   internal struct EntityMeta
    {
        public bool Active;
        public ushort Version;
        public ComponentMask256 ArchetypeMask;
        public int ViewID;
    }
}
