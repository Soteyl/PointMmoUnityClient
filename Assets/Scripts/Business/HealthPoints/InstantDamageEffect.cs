using System;
using Business.Effects;

namespace Business.HealthPoints
{
    public class InstantDamageEffect: IEffect<Health>
    {
        private readonly int _damage;

        public InstantDamageEffect(int damage)
        {
            _damage = damage;
        }
        
        public void Apply(Health entity)
        {
            entity.CurrentPoints -= _damage;
            EffectEnded?.Invoke(this, EventArgs.Empty);
        }

        public void Misapply(Health entity)
        { }

        public event EventHandler EffectEnded;
    }
}