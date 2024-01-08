using System;
using System.Collections;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy
{
    public class AttackCharacterOnTriggerComponent: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private CharacterTrigger _characterTrigger;

        [OdinSerialize]
        private Movement _enemyMovement;

        [OdinSerialize]
        private EnemyComponent _enemyComponent;
        
        private Coroutine _attackCoroutine;

        private void Awake()
        {
            _characterTrigger.CharacterTriggered += OnCharacterTriggered;
            _characterTrigger.CharacterLeavedTrigger += OnCharacterLeavedCharacterTrigger;
        }

        private void OnCharacterLeavedCharacterTrigger(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            if (_attackCoroutine is not null)
                StopCoroutine(_attackCoroutine);
        }

        private void OnCharacterTriggered(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            _attackCoroutine = StartCoroutine(AttackCharacter(e.Character));
        }
        
        private IEnumerator AttackCharacter(CharacterComponent character)
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (!character.Entity.Health.IsAlive || !_enemyComponent.Entity.Health.IsAlive) 
                    yield break;
                if (Vector3.Distance(_enemyComponent.transform.position, character.transform.position) > _enemyComponent.Entity.Weapon.WeaponData.Distance)
                {
                    _enemyMovement.MoveTo(GetPointNearTarget(character));
                }
                else _ = _enemyComponent.Entity.AttackAsync(character.Entity);
            }
        }
        
        private Vector3 GetPointNearTarget(EntityComponent targetEntity)
        {
            var position = targetEntity.transform.position;
            Vector3 directionToEntity = (position - _enemyComponent.transform.position).normalized;
            Vector3 destinationPoint = position - directionToEntity * (Math.Max(_enemyComponent.Entity.Weapon.WeaponData.Distance, _enemyMovement.StoppingDistance) - 0.5f);
            return destinationPoint;
        }
    }
}