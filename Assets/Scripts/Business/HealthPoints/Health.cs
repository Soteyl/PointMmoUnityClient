using System;
using Business.Effects;
using Business.Entities;
using Business.Multipliers;

namespace Business.HealthPoints
{
    public class Health
    {
        private float _currentPoints;

        private readonly IEffectsContainer<Health> _effects;
        
        private readonly Entity _entity;

        private bool _isAlive = true;

        public Health(IMultipliedValue maxHealth)
        {
            Max = maxHealth;
            
            Max.ValueChanged += (sender, f) => MaxHealthUpdated?.Invoke(this, new HealthUpdatedArgs(f.OldValue, f.NewValue, this));
            
            _effects = new EffectsContainer<Health>(this);
        }

        public float Current
        {
            get => Math.Clamp(_currentPoints, 0, Max.GetValue());
            set
            {
                float oldHealth = _currentPoints;
                _currentPoints = Math.Clamp(value, 0, Max.GetValue());
                CurrentHealthUpdated?.Invoke(this, new HealthUpdatedArgs(oldHealth, _currentPoints, this));
                if (oldHealth > 0 && _currentPoints == 0)
                {
                    _isAlive = false;
                    Died?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        
        public IMultipliedValue Max { get; }

        public bool IsAlive => _isAlive;

        public IEffectsContainer<Health> Effects => _effects;

        public event EventHandler<HealthUpdatedArgs> CurrentHealthUpdated;

        public event EventHandler<HealthUpdatedArgs> MaxHealthUpdated;

        public event EventHandler Died;

        public event EventHandler Resurrected;

        public void Resurrect()
        {
            _isAlive = true;
            _currentPoints = Max.GetValue();
            CurrentHealthUpdated?.Invoke(this, new HealthUpdatedArgs(0, _currentPoints, this));
            Resurrected?.Invoke(this, EventArgs.Empty);
        }
    }
}