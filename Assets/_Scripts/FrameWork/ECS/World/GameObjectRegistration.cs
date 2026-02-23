using System.Collections.Generic;
using UnityEngine;
namespace ECS
{
    internal class GameObjectRegistration
    {
        private Dictionary<int, GameObject> IDGameObjectMap = new();
        public int GetID(GameObject gameObject)
        {
            if (IDGameObjectMap == null)
            {
                Debug.LogError($"GameObjectRegistration Error:Cant Find IDGameObjectMap");
                return -1;
            }
            if (gameObject)
            {
                int index = gameObject.GetInstanceID();
                if (!IDGameObjectMap.ContainsKey(index))
                    IDGameObjectMap[index] = gameObject;
                return index;
            }
            return -1;
        }
        public GameObject GetGameObject(int id)
        {
            if (IDGameObjectMap == null)
            {
                Debug.LogError($"GameObjectRegistration Error:Cant Find IDGameObjectMap");
                return null;
            }
            if (IDGameObjectMap.TryGetValue(id,out GameObject go))
            {
                if (!go)
                {
                    IDGameObjectMap.Remove(id);
                    return null;
                }
                return go;
            }
            return null;
        }
        public GameObject GetGameObject(Entity entity) => GetGameObject(entity.GameObjectID);
        public void OnReleaseEntity(Entity entity)
        {
            if (IDGameObjectMap == null)
            {
                Debug.LogError($"GameObjectRegistration Error:Cant Find IDGameObjectMap");
                return;
            }
            if (!IDGameObjectMap.ContainsKey(entity.GameObjectID))
                Debug.LogError($"GameObjectRegistration Error:Haven't Registed This ID:{entity.GameObjectID} Before");
            //因为在整个生命周期里面GameObject的InstanceID都是唯一的以及避免重复注册，所以这里不删除对应Entity的对象
        }
        public void Dispose()
        {
            if(IDGameObjectMap!=null)
                IDGameObjectMap.Clear();
            IDGameObjectMap = null;
        }
    }
}
