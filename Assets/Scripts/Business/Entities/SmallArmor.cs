using Business.Effects;
using Business.HealthPoints;

namespace Business.Entities
{
    public class SmallArmor : IArmor
    {
        private readonly IEffect<Health> _healthEffect = new HealthMultiplierEffect(2);

        public void Apply(Entity entity)
        {
            entity.Health.Effects.AddEffect(_healthEffect);
        }

        public void Misapply(Entity entity)
        {
            entity.Health.Effects.RemoveEffect(_healthEffect);
        }
    }
}