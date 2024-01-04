using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Effects;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Business.HealthPoints
{
    public class IdleHealEffect: IEffect<Health>
    {
        private readonly TimeSpan _healDelay;
        
        private readonly TimeSpan _idleTime;
        
        private readonly float _pointsToHeal;

        private Health _health;

        private CancellationTokenSource _cancellationTokenSource;
        
        public event EventHandler EffectEnded;

        /// <param name="healDelay">Time between heal ticks</param>
        /// <param name="idleTime">Time after damage before start heal</param>
        /// <param name="pointsToHeal">Health points to heal every heal tick</param>
        public IdleHealEffect(float pointsToHeal, TimeSpan healDelay, TimeSpan idleTime)
        {
            _pointsToHeal = pointsToHeal;
            _healDelay = healDelay;
            _idleTime = idleTime;
        }
        
        public void Apply(Health entity)
        {
            _health = entity;
            entity.CurrentHealthUpdated += StartHealOnDamage;
            entity.Died += EntityOnDied;
            StartHealingTask();
        }

        public void Misapply(Health entity)
        {
            entity.CurrentHealthUpdated -= StartHealOnDamage;
            entity.Died -= EntityOnDied;
            StopHealingTask();
        }

        private void EntityOnDied(object sender, EventArgs e)
        {
            StopHealingTask();
        }

        private void StartHealOnDamage(object sender, HealthUpdatedArgs e)
        {
            if ((e.NewHealth < e.OldHealth || _cancellationTokenSource is null) && 
                e.NewHealth != e.Health.Max.GetValue())
                StartHealingTask();
        }

        private void StartHealingTask()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            HealWhileIdleAsync(_cancellationTokenSource.Token);
        }

        private void StopHealingTask()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }

        private async Task HealWhileIdleAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(_idleTime, cancellationToken);
            
            do
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await Task.Delay(_healDelay, cancellationToken);
                _health.Current += _pointsToHeal;
            } while (_health.Current != _health.Max.GetValue());
        }
    }
}