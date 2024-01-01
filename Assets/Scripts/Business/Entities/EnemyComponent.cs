using System;
using Business.HealthPoints;
using Components;
using UnityEngine;

namespace Business.Entities
{
    public class EnemyComponent: EntityComponent
    {
        public EnemyComponent() : base(new Entity())
        {
            Entity.Health.Died += HealthOnDied;
        }

        private void HealthOnDied(object sender, EventArgs e)
        {
            Destroy(this);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<EntityComponent>(out var entity))
            {
                entity.Entity.Health.Effects.AddEffect(new InstantDamageEffect(10));
            }
        }
    }
}