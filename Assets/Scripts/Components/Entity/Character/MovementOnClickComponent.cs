using Components.Interacting;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity.Character
{
    public class MovementOnClickComponent : SerializedMonoBehaviour
    {
        [OdinSerialize]
        private Movement _movementComponent;
        
        private ClickEventTransfer _clickEventTransfer;

        private void Awake()
        {
            _clickEventTransfer = ClickEventTransfer.FindInScene();
            _clickEventTransfer.InteractableObjectHold += OnMouseClick;
        }

        private void OnMouseClick(object sender, InteractableObjectEventArgs e)
        {
            if (e.Object.Type == InteractableObjectType.Floor)
                _movementComponent.MoveTo(e.HitPosition);
        }
    }
}