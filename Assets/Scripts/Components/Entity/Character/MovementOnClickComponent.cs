using System.Linq;
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
        
        private bool _isMoving;

        private void Awake()
        {
            _clickEventTransfer = ClickEventTransfer.FindInScene();
            _clickEventTransfer.InteractableObjectInvoked += MoveStarted;
            _clickEventTransfer.InteractableObjectReleased += MoveStopped;
            _clickEventTransfer.InteractableObjectHold += MoveContinued;
        }

        private void MoveStopped(object sender, InteractableObjectEventArgs e)
        {
            _isMoving = false;
        }

        private void MoveStarted(object sender, InteractableObjectEventArgs e)
        {
            if (e.Interaction.Object.Type == InteractableObjectType.Floor)
            {
                _isMoving = true;
                MoveContinued(sender, e);
            }
        }

        private void MoveContinued(object sender, InteractableObjectEventArgs e)
        {
            if (_isMoving && e.Interactions.FirstOrDefault(x => x.Object.Type == InteractableObjectType.Floor) is not null and var floor)
                _movementComponent.MoveTo(floor.HitPosition);
        }
    }
}