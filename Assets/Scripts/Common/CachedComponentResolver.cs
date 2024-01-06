using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class CachedComponentResolver<T> where T : MonoBehaviour
    {
        private readonly int _maxCachedObjects;
        
        private readonly Dictionary<GameObject, LinkedListNode<KeyValuePair<GameObject, T>>> _cache;
        
        private readonly LinkedList<KeyValuePair<GameObject, T>> _lruList;

        public CachedComponentResolver(int maxCachedObjects = 255)
        {
            _maxCachedObjects = maxCachedObjects;
            _cache = new Dictionary<GameObject, LinkedListNode<KeyValuePair<GameObject, T>>>();
            _lruList = new LinkedList<KeyValuePair<GameObject, T>>();
        }

        public T Resolve(Component component) => Resolve(component.gameObject);
        

        public T Resolve(GameObject gameObject)
        {
            if (_cache.TryGetValue(gameObject, out var node))
            {
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                return node.Value.Value;
            }

            var component = gameObject.GetComponent<T>();
            AddToCache(gameObject, component);
            return component;
        }
        
        public bool TryResolve(Component component, out T resolvedComponent) => TryResolve(component.gameObject, out resolvedComponent);

        public bool TryResolve(GameObject gameObject, out T component)
        {
            if (_cache.TryGetValue(gameObject, out var node))
            {
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                component = node.Value.Value;
                return true;
            }

            var isSuccess = gameObject.TryGetComponent(out component);
            AddToCache(gameObject, component);
            return isSuccess;
        }

        private void AddToCache(GameObject gameObject, T component)
        {
            if (_cache.Count >= _maxCachedObjects)
            {
                var lastNode = _lruList.Last;
                _cache.Remove(lastNode.Value.Key);
                _lruList.RemoveLast();
            }

            var newNode = new LinkedListNode<KeyValuePair<GameObject, T>>(
                new KeyValuePair<GameObject, T>(gameObject, component));
            _lruList.AddFirst(newNode);
            _cache[gameObject] = newNode;
        }
    }
}