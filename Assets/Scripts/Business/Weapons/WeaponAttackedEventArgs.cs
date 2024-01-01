using Business.Entities;

namespace Business.Weapons
{
    public class WeaponAttackedEventArgs
    {
        public WeaponAttackedEventArgs(Entity target, Entity attacker, IWeapon attackWeapon, int damage)
        {
            Target = target;
            AttackWeapon = attackWeapon;
            Damage = damage;
            Attacker = attacker;
        }

        public Entity Target { get; set; }
    
        public IWeapon AttackWeapon { get; set; }
        
        public Entity Attacker { get; set; }
    
        public int Damage { get; set; }
    }
}