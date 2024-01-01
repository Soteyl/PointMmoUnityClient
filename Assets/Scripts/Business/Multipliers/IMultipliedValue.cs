using System;
using System.Collections.Generic;

namespace Business.Multipliers
{
    public interface IMultipliedValue
    {
        IEnumerable<IValueMultiplier> Multipliers { get; }
        
        float DefaultValue { get; }
        
        event EventHandler<ValueChangedEventArgs> ValueChanged; 

        void AddMultiplier(IValueMultiplier multiplier);

        void RemoveMultiplier(IValueMultiplier multiplier);

        void UpdateDefaultValue(float newDefaultValue);

        float GetValue();
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public float OldValue { get; set; }
        
        public float NewValue { get; set; }
    }
}