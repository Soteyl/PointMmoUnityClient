using System;
using System.Collections;
using Business.Entities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Components
{
    public class Movement: MonoBehaviour
    {
        private Coroutine _moveCoroutine;
        
        [SerializeField]
        private EntityComponent entity;
        
        [SerializeField]
        private NavMeshAgent navMeshAgent;

        private Action<MovementStatus> _actionOnFinishMove;

        public float StoppingDistance => navMeshAgent.stoppingDistance;

        private void Awake()
        {
            navMeshAgent.speed = entity.Entity.Speed.GetValue();
            
            entity.Entity.Health.Died += OnCharacterDied;
            entity.Entity.Health.Resurrected += OnCharacterResurrected;
        }

        public void MoveTo(Vector3 position)
        {
            MoveToAndThen(position, null);
        }

        public void MoveToAndThen(Vector3 position, Action<MovementStatus> action)
        {
            StopActiveMovement();
            _actionOnFinishMove = action;
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(position);
            _moveCoroutine = StartCoroutine(WaitUntilFinishMove());
        }

        private IEnumerator WaitUntilFinishMove()
        {
            do yield return new WaitForFixedUpdate();
            while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance * 1.1f);

            navMeshAgent.ResetPath();
            
            _actionOnFinishMove?.Invoke(MovementStatus.Finished);
            _actionOnFinishMove = null;
        }

        private void StopActiveMovement()
        {
            if (_moveCoroutine is null) return;
            
            _actionOnFinishMove?.Invoke(MovementStatus.Canceled);
            _actionOnFinishMove = null;
            StopCoroutine(_moveCoroutine);
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
            _moveCoroutine = null;
        }
        
        private void OnCharacterDied(object sender, EventArgs e)
        {
            navMeshAgent.isStopped = true;
        }
        
        private void OnCharacterResurrected(object sender, EventArgs e)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = false;
        }
        
    }

    public enum MovementStatus
    {
        Finished,
        Canceled
    }
}