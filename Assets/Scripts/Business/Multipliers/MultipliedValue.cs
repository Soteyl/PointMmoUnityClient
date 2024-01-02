using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Business.Multipliers
{
    public class MultipliedValue: IMultipliedValue
    {
        private readonly float _max;
        
        private readonly float _min;

        private readonly List<IValueMultiplier> _multipliers = new();
        
        private float _currentValue;

        public MultipliedValue(float defaultValue, float min = float.MinValue, float max = float.MaxValue)
        {
            DefaultValue = defaultValue;
            _max = max;
            _min = min;
            _currentValue = defaultValue;
        }
        
        public static implicit operator float(MultipliedValue multipliedValue) => multipliedValue.GetValue();

        public IEnumerable<IValueMultiplier> Multipliers => _multipliers;
        
        public float DefaultValue { get; private set;  }

        public event EventHandler<ValueChangedEventArgs> ValueChanged; 

        public void AddMultiplier(IValueMultiplier multiplier)
        {
            _multipliers.Add(multiplier);
            Recount();
        }

        public void RemoveMultiplier(IValueMultiplier multiplier)
        {
            _multipliers.Remove(multiplier);
            Recount();
        }

        public void UpdateDefaultValue(float newDefaultValue)
        {
            DefaultValue = newDefaultValue;
            Recount();
        }

        public float GetValue() => _currentValue;

        private void Recount()
        {
            var oldValue = _currentValue;
            _currentValue = Math.Clamp(_multipliers.Aggregate(DefaultValue,
                    (current, healthMultiplier) => (int)Math.Round(healthMultiplier.Apply(current))),
                _min,
                _max);
            ValueChanged?.Invoke(this, new ValueChangedEventArgs
            {
                OldValue = oldValue,
                NewValue = _currentValue
            });
        }

        public override string ToString() => _currentValue.ToString(CultureInfo.InvariantCulture);
    }
}