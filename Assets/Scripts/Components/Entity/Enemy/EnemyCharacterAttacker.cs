using System.Collections.Generic;
using Components.Entity.Character;
using MEC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Entity.Enemy
{
    public class EnemyCharacterAttacker: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private Movement _enemyMovement;

        [OdinSerialize]
        private EnemyComponent _enemyComponent;
        
        private CoroutineHandle _attackCoroutine;

        public void StartAttack(CharacterComponent character)
        {
            _attackCoroutine = Timing.RunCoroutineSingleton(_AttackCharacter(character).CancelWith(this), 
                _attackCoroutine, SingletonBehavior.Overwrite);
        }
        
        public void StopAttack()
        {
            Timing.KillCoroutines(_attackCoroutine);
        }
        
        private IEnumerator<float> _AttackCharacter(CharacterComponent character)
        {
            while (true)
            {
                yield return Timing.WaitForOneFrame;
                if (!character.Entity.Health.IsAlive || !_enemyComponent.Entity.Health.IsAlive) 
                    yield break;
                if (Vector3.Distance(_enemyComponent.transform.position, character.transform.position) > _enemyComponent.Entity.Weapon.WeaponData.Distance)
                {
                    _enemyMovement.MoveTo(new MoveRequest()
                    {
                            VectorTarget = character.transform.position,
                            StoppingDistance = _enemyComponent.Entity.Weapon.WeaponData.Distance
                    });
                }
                else _ = _enemyComponent.Entity.AttackAsync(character.Entity);
            }
        }
    }
}