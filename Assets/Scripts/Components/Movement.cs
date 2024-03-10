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

        public void MoveToAndThen(Vector3 position, Action<MovementStatus> action)
        {
            _actionOnFinishMove?.Invoke(MovementStatus.Canceled); 
            
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