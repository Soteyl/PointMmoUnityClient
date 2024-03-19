using System;
using System.Collections.Generic;
using Business.Multipliers;
using Components;
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

        public void MoveTo(MoveRequest request)
        {
            _actionOnFinishMove?.Invoke(MovementStatus.Canceled);
            _actionOnFinishMove = request.OnFinish;

            IEnumerator<float> coroutine;
            if (request.VectorTarget.HasValue)
                coroutine = _WaitUntilFinishMove(() => request.VectorTarget.Value, request.StoppingDistance).CancelWith(gameObject);
            else
                coroutine = _WaitUntilFinishMove(() => request.TransformTarget.position, request.StoppingDistance)
                        .CancelWith(gameObject, request.TransformTarget.gameObject);
            
            _moveCoroutine = Timing.RunCoroutineSingleton(coroutine, _moveCoroutine,
                Segment.FixedUpdate, SingletonBehavior.Overwrite); 
        }

        private IEnumerator<float> _WaitUntilFinishMove(Func<Vector3> positionFunc, float? stoppingDistance = null)
        {
            do
            {
                var position = stoppingDistance.HasValue
                        ? GetPointNearTarget(transform.position, positionFunc(), stoppingDistance.Value) : positionFunc();
                navMeshAgent.SetDestination(position);
                
                yield return Timing.WaitForOneFrame;
            }
            while (navMeshAgent.isActiveAndEnabled && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance * 1.02f);
                
            if (navMeshAgent.isActiveAndEnabled)
                navMeshAgent.ResetPath();

            _actionOnFinishMove?.Invoke(MovementStatus.Finished);
            _actionOnFinishMove = null;
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
    
    
    public class MoveRequest
    {
        /// <summary> Should to be set if <see cref="TransformTarget"/> is not set </summary>
        public Vector3? VectorTarget { get; set; }
    
        /// <summary> Should to be set if <see cref="VectorTarget"/> is not set </summary>
        public Transform TransformTarget { get; set; }
    
        public Action<MovementStatus> OnFinish { get; set; }
    
        public float? StoppingDistance { get; set; }
    }
}
