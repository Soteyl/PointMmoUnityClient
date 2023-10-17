using Business.Entities;
using Business.HealthPoints;
using UnityEngine;

namespace Components
{
    public class EnemyComponent: EntityComponent
    {
        public EnemyComponent() : base(new Entity())
        { }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<EntityComponent>(out var entity))
            {
                entity.Entity.Health.Effects.AddEffect(new InstantDamageEffect(10));
            }
        }
    }
}