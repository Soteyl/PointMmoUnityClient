using Common;
using Components.Entity.Enemy;
using Components.Interacting;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

#pragma warning disable CS4014

namespace Components.Entity.Character
{
    public class AttackOnClickComponent: SerializedMonoBehaviour
    {
        private readonly CachedComponentResolver<EnemyComponent> _cachedEnemy = new();

        [OdinSerialize]
        private CharacterComponent Character { get; set; }
        
        [OdinSerialize]
        private Movement _movementComponent;
        
        private ClickEventTransfer _clickEventTransfer;

        private void Awake()
        {
            _clickEventTransfer = ClickEventTransfer.FindInScene();
            _clickEventTransfer.InteractableObjectInvoked += MouseClicked;
        }

        private void MouseClicked(object sender, InteractableObjectEventArgs e)
        {
            if (e.Interaction.Object.Type == InteractableObjectType.Enemy && _cachedEnemy.TryResolve(e.Interaction.Object, out var entity))
                _movementComponent.MoveTo(new MoveRequest()
                {
                        TransformTarget = entity.transform,
                        OnFinish = (st) =>
                        {
                            if (st == MovementStatus.Finished) Character.Entity.AttackAsync(entity.Entity);
                        },
                        StoppingDistance = Character.Entity.Weapon.WeaponData.Distance
                });
        }
        
        private void OnDestroy()
        {
            _clickEventTransfer.InteractableObjectInvoked -= MouseClicked;
        }
    }
}