using System.Collections;
using Business.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    public class Movement: MonoBehaviour
    {
        public CharacterComponent character;
        private NavMeshAgent _navMeshAgent;
        private Coroutine _moveCoroutine;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = character.Entity.Speed.GetValue();
        }
        
        private IEnumerator WaitUntilFinishMove()
        {
            yield return new WaitForFixedUpdate();
            while (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
            {
                yield return new WaitForFixedUpdate();
            }

            _navMeshAgent.ResetPath();
        }
    }
}