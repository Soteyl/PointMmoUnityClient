using System;
using Business.Effects;
using Business.Entities;

namespace Business.HealthPoints
{
    public class InstantDamageEffect: IEffect<Health>
    {
        private readonly int _damage;

        public InstantDamageEffect(int damage, Entity creator = null)
        {
            _damage = damage;
            Creator = creator;
        }

        public Entity Creator { get; }

        public void Apply(Health entity)
        {
            entity.Current -= _damage;
            EffectEnded?.Invoke(this, EventArgs.Empty);
        }

        public void Misapply(Health entity)
        { }

        public event EventHandler EffectEnded;
    }
}