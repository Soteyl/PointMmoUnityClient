using System;
using System.Collections;
using System.Collections.Generic;
using Business.Multipliers;
using Components.Entity;
using MEC;
using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    public class Movement: MonoBehaviour
    {
        private CoroutineHandle _moveCoroutine;
        
        [SerializeField]
        private EntityComponent entity;
        
        [SerializeField]
        private NavMeshAgent navMeshAgent;

        private Action<MovementStatus> _actionOnFinishMove;

        public float StoppingDistance => navMeshAgent.stoppingDistance;

        private void Awake()
        {
            navMeshAgent.speed = entity.Entity.Speed.GetValue();
            
            entity.Entity.Speed.ValueChanged += OnSpeedChanged;
        }

        private void OnSpeedChanged(object sender, ValueChangedEventArgs e)
        {
            navMeshAgent.speed = entity.Entity.Speed.GetValue();
            
            if (e.OldValue == 0)
                navMeshAgent.ResetPath();
        }

        public void MoveTo(Vector3 position)
        {
            MoveToAndThen(position, null);
        }

        // todo move to transform for attacks
        public void MoveToAndThen(Vector3 position, Action<MovementStatus> action, float? stoppingDistance = null)
        {
            _actionOnFinishMove?.Invoke(MovementStatus.Canceled); 
            
            if (stoppingDistance.HasValue)
                position = GetPointNearTarget(transform.position, position, stoppingDistance.Value);
            
            navMeshAgent.SetDestination(position);

            _actionOnFinishMove = action;
            _moveCoroutine = Timing.RunCoroutineSingleton(_WaitUntilFinishMove().CancelWith(this), _moveCoroutine,
                Segment.FixedUpdate, SingletonBehavior.Overwrite); 
        }

        private IEnumerator<float> _WaitUntilFinishMove()
        {
            do yield return Timing.WaitForOneFrame;
            while (navMeshAgent.isActiveAndEnabled && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance * 1.02f);
                
            if (navMeshAgent.isActiveAndEnabled)
                navMeshAgent.ResetPath();

            _actionOnFinishMove?.Invoke(MovementStatus.Finished);
        }
        
        private Vector3 GetPointNearTarget(Vector3 currentPosition, Vector3 targetPosition, float stoppingDistance)
        {
            Vector3 directionToEntity = (targetPosition - currentPosition).normalized;
            Vector3 destinationPoint = targetPosition - directionToEntity * (Math.Max(stoppingDistance, StoppingDistance) - 0.5f);
            return destinationPoint;
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_moveCoroutine);
        }
    }

    public enum MovementStatus
    {
        Finished,
        Canceled
    }
}