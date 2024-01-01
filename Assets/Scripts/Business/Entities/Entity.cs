using System.Threading;
using System.Threading.Tasks;
using Business.HealthPoints;
using Business.Multipliers;
using Business.Weapons;

namespace Business.Entities
{
    public class Entity
    {
        protected readonly Health _health;

        protected readonly IMultipliedValue _speed;

        protected IWeapon _weapon;

        private IValueMultiplier _deadSpeedMultiplier = new DefaultValueMultiplier(0);

        public Entity()
        {
            _health = new Health(new MultipliedValue(100, min: 1));
            _speed = new MultipliedValue(3.5f, min: 0f);

            _health.Died += (_, _) => _speed.AddMultiplier(_deadSpeedMultiplier);
            _health.Resurrected += (_, _) => _speed.RemoveMultiplier(_deadSpeedMultiplier);
        }

        public Health Health => _health;

        public IMultipliedValue Speed => _speed;

        public IWeapon Weapon => _weapon;

        /// <param name="weapon">new weapon</param>
        /// <returns>Old weapon</returns>
        public IWeapon EquipWeapon(IWeapon weapon)
        {
            var oldWeapon = _weapon;
            _weapon = weapon;
            return oldWeapon;
        }

        public async Task AttackAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            await _weapon.AttackAsync(this, entity, cancellationToken);
        }
    }
}
