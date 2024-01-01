using System;

namespace Business.HealthPoints
{
    public class HealthUpdatedArgs: EventArgs
    {
        public HealthUpdatedArgs(float oldHealth, float newHealth, Health health)
        {
            OldHealth = oldHealth;
            NewHealth = newHealth;
            Health = health;
        }

        public float OldHealth { get; set; }
        
        public float NewHealth { get; set; }
        
        public Health Health { get; set; }
    }
}