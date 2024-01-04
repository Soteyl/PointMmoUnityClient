using System;
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
        void Apply(T entity);

        void Misapply(T entity);
    }
}