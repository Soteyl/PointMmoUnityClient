using System.Collections.Generic;
using Common.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Hub
{
    public abstract class ComponentHub<THub>: SerializedMonoBehaviour
            where THub: ComponentHub<THub>
    {
        private static Dictionary<GameObject, ComponentHub<THub>> _hubAccessors = new();
        
        protected void Awake()
        {
            _hubAccessors.Add(gameObject, this);
            foreach (var child in transform.GetAllNestedChildren())
            {
                _hubAccessors.Add(child.gameObject, this);
            }
        }

        public static bool TryGet(GameObject gameObject, out THub hub)
        {
            hub = null;

            if (!_hubAccessors.ContainsKey(gameObject) || _hubAccessors[gameObject] is not THub foundHub) 
                return false;
            
            hub = foundHub;
            return true;
        }
    }
}