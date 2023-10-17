using System;
using System.Collections.Generic;
using System.Linq;
using Business.Effects;
using Business.Entities;
using Business.Multipliers;

namespace Business.HealthPoints
{
    public class Health
    {
        private readonly List<IValueMultiplier> _multipliers = new();

        private readonly EffectsContainer<Health> _effects;
        
        private readonly int _minPointsAfterMultiply;
        
        private readonly int _maxPointsAfterMultiply;

        private readonly Entity _entity;

        private int _currentPoints;
        
        private int _defaultPoints;

        public Health(int defaultPoints, int minPointsAfterMultiply = 1, int maxPointsAfterMultiply = int.MaxValue)
        {
            _defaultPoints = defaultPoints;
            _minPointsAfterMultiply = minPointsAfterMultiply;
            _maxPointsAfterMultiply = maxPointsAfterMultiply;
            _effects = new EffectsContainer<Health>(this);
            MaxPoints = _defaultPoints;
            _currentPoints = MaxPoints;
        }
        
        public int MaxPoints { get; private set; }

        public int CurrentPoints
        {
            get => Math.Clamp(_currentPoints, 0, MaxPoints);
            set
            {
                int oldHealth = _currentPoints;
                _currentPoints = Math.Clamp(value, 0, MaxPoints);
                CurrentHealthUpdated?.Invoke(this, new HealthUpdatedArgs(oldHealth, _currentPoints, this));
            } 
        }
    
        public int DefaultPoints => _defaultPoints;

        public EffectsContainer<Health> Effects => _effects;

        public event EventHandler<HealthUpdatedArgs> CurrentHealthUpdated;

        public event EventHandler<HealthUpdatedArgs> MaxHealthUpdated; 

        public void SetDefaultMaxPoints(int defaultPoints)
        {
            _defaultPoints = defaultPoints;
            RecountMaxPoints();
        }
    
        public void AddMultiplier(IValueMultiplier multiplier)
        {
            _multipliers.Add(multiplier);
            RecountMaxPoints();
        }
    
        public void RemoveMultiplier(IValueMultiplier multiplier)
        {
            _multipliers.Remove(multiplier);
            RecountMaxPoints();
        }

        private void RecountMaxPoints()
        {
            int oldHealth = MaxPoints;
            MaxPoints = Math.Clamp(_multipliers.Aggregate(_defaultPoints,
                (current, healthMultiplier) => (int)Math.Round(healthMultiplier.Apply(current))), _minPointsAfterMultiply, _maxPointsAfterMultiply);
            MaxHealthUpdated?.Invoke(this, new HealthUpdatedArgs(oldHealth, MaxPoints, this));
        }
    }
}