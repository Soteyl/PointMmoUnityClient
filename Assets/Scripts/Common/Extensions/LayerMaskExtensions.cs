using UnityEngine;

namespace Common.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask source, int layer)
        {
            return source == (source | (1 << layer));
        }
    }

    public enum Layer
    {
        Floor = 6,
        Enemy = 7,
        SpawnPoints = 8
    }
}