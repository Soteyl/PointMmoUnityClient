using System;
using Business.Entities;
using UnityEngine;

namespace Business.Effects
{
    public interface IEffect
    {
        /// <summary>
        /// On effect ended by itself
        /// </summary>
        event EventHandler EffectEnded;
    }
    
    public interface IEffect<in T> : IEffect
    {
        Entity Creator { get; }
        
        void Apply(T entity);

        void Misapply(T entity);
    }
}