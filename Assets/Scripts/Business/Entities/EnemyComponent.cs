using System;

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
            Destroy(gameObject);
        }
    }
}