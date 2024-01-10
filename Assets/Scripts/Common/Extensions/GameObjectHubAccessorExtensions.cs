using Common.Hub;
using UnityEngine;

namespace Common.Extensions
{
    public static class GameObjectHubAccessorExtensions
    {
        public static bool TryAccessHub<T>(this GameObject gameObject, out T hub)
                where T: ComponentHub<T>
        {
            return ComponentHub<T>.TryGet(gameObject, out hub);
        }
    }
}