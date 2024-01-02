using System;
using Business.Entities;
using Components;
using Components.Interacting;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
#pragma warning disable CS4014

namespace Business.Weapons
{
    public class AttackOnClickComponent: SerializedMonoBehaviour
    {
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

        private void MouseClicked(object sender, InteractableObjectInvokedEventArgs e)
        {
            if (e.Object.Type == InteractableObjectType.Enemy && e.Object.TryGetComponent<EnemyComponent>(out var entity))
                _movementComponent.MoveToAndThen(GetPointNearTarget(entity), (st) =>
                {
                    if (st == MovementStatus.Finished) Character.Entity.AttackAsync(entity.Entity);
                });
        }
        
        private Vector3 GetPointNearTarget(EntityComponent targetEntity)
        {
            var position = targetEntity.transform.position;
            Vector3 directionToEntity = (position - Character.transform.position).normalized;
            Vector3 destinationPoint = position - directionToEntity * (Math.Max(Character.Entity.Weapon.WeaponData.Distance, _movementComponent.StoppingDistance) - 0.5f);
            return destinationPoint;
        }
    }
}