using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    public sealed class ComponentTypeRegistry
    {
        private readonly Dictionary<Type, ComponentTypeID> _typeToID = new();
        private readonly List<Type> _idToType = new();
        public int TypeCount => _idToType.Count;
        public ComponentTypeID Register<T>() where T : Component => Register(typeof(T));
        public ComponentTypeID Register(Type type)
        {
            if (type == null)
            {
                Debug.LogError("ComponentTypeRegistry Register Error:Type Is Null");
                return default;
            }
            if (_typeToID.TryGetValue(type, out ComponentTypeID ctid))
                return ctid;
            ushort idValue = checked((ushort)TypeCount);//·ÀÖ¹¾²Ä¬½Ø¶Ï
            ComponentTypeID id = new(idValue);
            _typeToID.Add(type, id);
            _idToType.Add(type);
            return id;
        }
        public ComponentTypeID Get<T>() where T : Component => Get(typeof(T));
        public ComponentTypeID Get(Type type)
        {
            if (type == null)
            {
                Debug.LogError("ComponentTypeRegistry Get Error:Type Is Null");
                return default;
            }
            if (!_typeToID.TryGetValue(type, out ComponentTypeID id))
            {
                Debug.LogError($"ComponentTypeRegistry Get Error:Cant Find The Key:{type.Name}");
                return default;
            }
            return id;
        }
        public Type GetRuntimeType(ComponentTypeID id)
        {
            if (id.Value < 0 || id.Value >= TypeCount)
            {
                Debug.LogError($"ComponentTypeRegistry GetRuntimeType Error:ID.Value:{id.Value} Is Out Of Range:[0,{TypeCount})");
                return null;
            }
            return _idToType[id.Value];
        }
    }
}
