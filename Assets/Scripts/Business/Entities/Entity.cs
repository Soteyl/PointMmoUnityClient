using Business.HealthPoints;

namespace Business.Entities
{
    public class Entity
    {
        private Health _health;

        public Entity()
        {
            _health = new Health(100);
        }

        public Health Health => _health;
    }
}
