using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Multipliers;
using Components.Entity;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    public class Movement: MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;
        
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
            entity.Entity.Speed.ValueChanged += OnSpeedChanged;
        }

        private void OnSpeedChanged(object sender, ValueChangedEventArgs e)
        {
            navMeshAgent.speed = entity.Entity.Speed.GetValue();
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
            _cancellationTokenSource = new CancellationTokenSource();
            _ = WaitUntilFinishMoveAsync(_cancellationTokenSource.Token);
        }

        private async Task WaitUntilFinishMoveAsync(CancellationToken cancellationToken = default)
        {
            do await UniTask.DelayFrame(1, cancellationToken: cancellationToken);
            while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance * 1.02f);

            if (cancellationToken.IsCancellationRequested)
                return;

            navMeshAgent.ResetPath();

            _actionOnFinishMove?.Invoke(MovementStatus.Finished);
            _actionOnFinishMove = null;
        }

        private void StopActiveMovement()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            
            _actionOnFinishMove?.Invoke(MovementStatus.Canceled);
            _actionOnFinishMove = null;
            
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
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