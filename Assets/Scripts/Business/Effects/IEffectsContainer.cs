using System;
using System.Collections.Generic;

namespace Business.Effects
{
    public interface IEffectsContainer<T> where T: class
    {
        IEnumerable<IEffect<T>> Effects { get; }
        
        event EventHandler<IEffect<T>> EffectAdded;

        event EventHandler<IEffect<T>> EffectRemoved;

        void AddEffect(IEffect<T> effect);

        void RemoveEffect(IEffect<T> effect);
    }
}