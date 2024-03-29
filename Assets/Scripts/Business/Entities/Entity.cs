using System;
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

        private readonly IValueMultiplier _deadSpeedMultiplier = new DefaultValueMultiplier(0);

        protected IWeapon _weapon;

        public Entity()
        {
            _health = new Health(new MultipliedValue(100, min: 1));
            _health.Effects.AddEffect(new IdleHealEffect(3, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(7)));
            _speed = new MultipliedValue(3.5f, min: 0f);

            _health.Died += (_, _) => _speed.AddMultiplier(_deadSpeedMultiplier);
            _health.Resurrected += (_, _) => _speed.RemoveMultiplier(_deadSpeedMultiplier);
        }

        public Health Health => _health;

        public IMultipliedValue Speed => _speed;

        public IWeapon Weapon => _weapon; // todo weapon manager
        
        public event EventHandler<WeaponAttackedEventArgs> Attacked;

        /// <param name="weapon">new weapon</param>
        /// <returns>Old weapon</returns>
        public IWeapon EquipWeapon(IWeapon weapon)
        {
            var oldWeapon = _weapon;
            if (_weapon != null)
                _weapon.Attacked -= WeaponOnAttacked;
            
            _weapon = weapon;
            if (_weapon != null)
                _weapon.Attacked += WeaponOnAttacked;
            
            return oldWeapon;
        }

        private void WeaponOnAttacked(object sender, WeaponAttackedEventArgs e)
        {
            Attacked?.Invoke(this, e);
        }

        public async Task AttackAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            if (Health.IsAlive)
            {
                await _weapon.AttackAsync(this, entity, cancellationToken);
            }
        }
    }
}
