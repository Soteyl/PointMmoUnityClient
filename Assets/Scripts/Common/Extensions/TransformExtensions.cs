using System.Collections.Generic;
using UnityEngine;

namespace Common.Extensions
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> GetAllNestedChildren(this Transform transform)
        {
            var queue = new Queue<Transform>();

            foreach (Transform child in transform)
            {
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                yield return current;

                foreach (Transform child in current)
                {
                    queue.Enqueue(child);
                }
            }
        }
    }
}