using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Interacting
{
    public class InteractableObject: SerializedMonoBehaviour
    {
        [OdinSerialize]
        public InteractableObjectType Type { get; private set; }
    }
}