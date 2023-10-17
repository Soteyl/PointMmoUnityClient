using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(BoxCollider))]
    public class SpawnPointComponent : SerializedMonoBehaviour
    {
        [OdinSerialize] private Vector3 _point { get; set; }

        public Vector3 Point => _point + transform.position;

        private void OnDrawGizmos()
        {
            var oldColor = Gizmos.color;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Point, 1);
            Gizmos.color = oldColor;
        }
    }
}