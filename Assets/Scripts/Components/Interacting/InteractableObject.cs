using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Interacting
{
    public class InteractableObject: SerializedMonoBehaviour
    {
        public const int Layer = 3;
        
        [OdinSerialize]
        public InteractableObjectType Type { get; private set; }

        private void Awake()
        {
            gameObject.layer = Layer;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            gameObject.layer = Layer;
        }
    }
}