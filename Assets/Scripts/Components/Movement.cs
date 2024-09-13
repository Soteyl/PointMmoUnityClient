using System;
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
        private bool _isMoving;
        
        [SerializeField]
        private EntityComponent entity;
        
        [SerializeField]
        private NavMeshAgent navMeshAgent;

        private Action<MovementStatus> _actionOnFinishMove;

        public bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (value != _isMoving)
                    MoveStatusChanged?.Invoke(this, new MoveStatusChangedEventArgs(){ IsMoving = value });
                _isMoving = value;
            }
        }

        public event EventHandler<MoveStatusChangedEventArgs> MoveStatusChanged;

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
            var position = stoppingDistance.HasValue
                    ? GetPointNearTarget(transform.position, positionFunc(), stoppingDistance.Value) : positionFunc();
            navMeshAgent.SetDestination(position);
            
            if (!IsNearTarget())
                IsMoving = true;
            
            while (navMeshAgent.isActiveAndEnabled && !IsNearTarget())
            {
                yield return Timing.WaitForOneFrame;
                
                position = stoppingDistance.HasValue
                    ? GetPointNearTarget(transform.position, positionFunc(), stoppingDistance.Value) : positionFunc();
                navMeshAgent.SetDestination(position);
            }
                
            if (navMeshAgent.isActiveAndEnabled)
                navMeshAgent.ResetPath();

            _actionOnFinishMove?.Invoke(MovementStatus.Finished);
            _actionOnFinishMove = null;
            IsMoving = false;
        }
        
        private bool IsNearTarget()
        {
            return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance * 1.02f;
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
            IsMoving = false;
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

    public class MoveStatusChangedEventArgs : EventArgs
    {
        public bool IsMoving { get; set; }
    }
}
