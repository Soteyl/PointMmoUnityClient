using System;
using Business.Effects;
using Business.Entities;

namespace Business.HealthPoints
{
    public class InstantHealEffect: IEffect<Health>
    {
        private readonly float _heal;

        public InstantHealEffect(float heal, Entity creator = null)
        {
            _heal = heal;
            Creator = creator;
        }

        public Entity Creator { get; }

        public void Apply(Health entity)
        {
            entity.Current += _heal;
            EffectEnded?.Invoke(this, EventArgs.Empty);
        }

        public void Misapply(Health entity)
        { }

        public event EventHandler EffectEnded;
    }
}