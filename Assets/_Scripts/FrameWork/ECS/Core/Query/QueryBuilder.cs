using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    public sealed class QueryBuilder
    {
        private ComponentTypeRegistry _registry;
        private ComponentMask256 _include;
        private ComponentMask256 _exclude;
        public QueryBuilder(ComponentTypeRegistry typeRegistry)=>_registry = typeRegistry;
        public QueryBuilder With<T>() where T : Component
        {
            _include.Set(_registry.Get<T>());
            return this;
        }
        public QueryBuilder Without<T>() where T : Component
        {
            _exclude.Set(_registry.Get<T>());
            return this;
        }
        public QueryMask Build() => new QueryMask(_include, _exclude);
    }
}
