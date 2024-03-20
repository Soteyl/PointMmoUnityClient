using System.Collections.Generic;
using Components.Entity.Enemy;
using Components.Entity.Enemy.Behaviour;
using MEC;
using UnityEngine;

namespace Components.Entity.Behaviour
{
    public class EnemyIdleState: IEntityBehaviourState
    {
        private readonly EnemyBehaviour _enemyBehaviour;
        
        private readonly EnemyHub _enemyHub;
        
        private readonly CoroutineHandle _coroutine;

        public EnemyIdleState(EnemyBehaviour enemyBehaviour, EnemyHub enemyHub)
        {
            _enemyBehaviour = enemyBehaviour;
            _enemyHub = enemyHub;
            _enemyHub.CharacterTriggerRunner.CharacterTriggered += OnCharacterTriggered;

            _coroutine = Timing.RunCoroutineSingleton(_WalkAround(), _coroutine, SingletonBehavior.Overwrite);
        }

        private IEnumerator<float> _WalkAround()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(Random.Range(2f, 5f));
                
                Vector3 randomPosition = _enemyHub.Enemy.SpawnPoint + Random.insideUnitSphere * 5;
                randomPosition.y = _enemyHub.Enemy.SpawnPoint.y;
                
                _enemyHub.Movement.MoveTo(new MoveRequest()
                {
                        VectorTarget = randomPosition
                });
            }
        }

        private void OnCharacterTriggered(object sender, TriggeredCharacterEnemyEventArgs e)
        {
            _enemyBehaviour.Attack(e.Character);
        }

        public void Dispose()
        {
            _enemyHub.CharacterTriggerRunner.CharacterTriggered -= OnCharacterTriggered;
            Timing.KillCoroutines(_coroutine);
        }
    }
}