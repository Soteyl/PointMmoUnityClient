﻿using System;
using System.Collections.Generic;

namespace Business.Effects
{
    public class EffectsContainer<T>: IEffectsContainer<T>
        where T: class
    {
        private readonly T _effectEntity;
        
        private readonly List<IEffect<T>> _effects = new();

        public EffectsContainer(T effectEntity = null)
        {
            _effectEntity = effectEntity;
        }

        public IEnumerable<IEffect<T>> Effects => _effects;
        
        public event EventHandler<IEffect<T>> EffectAdded;

        public event EventHandler<IEffect<T>> EffectRemoved;
        
        public void AddEffect(IEffect<T> effect)
        {
            _effects.Add(effect);
            EffectAdded?.Invoke(this, effect);
            
            effect.EffectEnded += OnEffectEnded;
            if (_effectEntity is not null)
                effect.Apply(_effectEntity);
        }

        public void RemoveEffect(IEffect<T> effect)
        {
            _effects.Remove(effect);
            EffectRemoved?.Invoke(this, effect);
            
            if (_effectEntity is not null)
                effect.Misapply(_effectEntity);
        }
        
        private void OnEffectEnded(object sender, EventArgs e)
        {
            RemoveEffect(sender as IEffect<T>);
        }
    }
}