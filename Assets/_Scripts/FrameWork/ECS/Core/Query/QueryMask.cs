using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    public readonly struct QueryMask
    {
        public readonly ComponentMask256 Include;
        public readonly ComponentMask256 Exclude;
        public QueryMask(ComponentMask256 include, ComponentMask256 exclude) => (Include, Exclude) = (include, exclude);
        public bool Matches(in ComponentMask256 entityMask) => entityMask.ContainsAll(Include) && !entityMask.ContainsAny(Exclude);
    }
}
