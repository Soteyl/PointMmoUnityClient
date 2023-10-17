using System;

namespace Business.HealthPoints
{
    public class HealthUpdatedArgs: EventArgs
    {
        public HealthUpdatedArgs(int oldHealth, int newHealth, Health health)
        {
            OldHealth = oldHealth;
            NewHealth = newHealth;
            Health = health;
        }

        public int OldHealth { get; set; }
        
        public int NewHealth { get; set; }
        
        public Health Health { get; set; }
    }
}