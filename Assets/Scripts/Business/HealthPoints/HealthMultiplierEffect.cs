using System;
using Business.Effects;
using Business.Entities;
using Business.Multipliers;

namespace Business.HealthPoints
{
    public class HealthMultiplierEffect : IEffect<Health>
    {
        private readonly DefaultValueMultiplier _multiplier;

        public HealthMultiplierEffect(float multiplier, Entity creator = null)
        {
            _multiplier = new DefaultValueMultiplier(multiplier);
            Creator = creator;
        }

        public Entity Creator { get; }

        public void Apply(Health entity)
        {
            entity.Max.AddMultiplier(_multiplier);
        }

        public void Misapply(Health entity)
        {
            entity.Max.RemoveMultiplier(_multiplier);
        }

        public event EventHandler EffectEnded;
    }
}