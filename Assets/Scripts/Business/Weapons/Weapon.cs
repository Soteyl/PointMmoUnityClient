using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Business.Entities;
using Business.HealthPoints;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Business.Weapons
{
    public class Weapon: IWeapon
    {
        private const int CoyoteTimeMilliseconds = 200;
        
        private readonly Stopwatch _stopwatch = new();
        
        private readonly Random _random = new();

        public Weapon(WeaponData weaponData)
        {
            WeaponData = weaponData;
            _stopwatch.Start();
        }
        
        public WeaponData WeaponData { get; }
        
        public bool IsReloading => _stopwatch.ElapsedMilliseconds < Delay;
        
        public bool IsPendingToAttack { get; private set; }
        
        private float Delay => 1000 / WeaponData.Speed;

        public virtual async Task AttackAsync(Entity owner, Entity target, CancellationToken cancellationToken = default)
        {
            if (IsPendingToAttack || _stopwatch.ElapsedMilliseconds < Delay - CoyoteTimeMilliseconds)
                return;
            
            if (IsReloading)
            {
                IsPendingToAttack = true;
                await Task.Delay((int)Mathf.Round(Delay - _stopwatch.ElapsedMilliseconds), cancellationToken);
            }

            int damage = _random.Next(WeaponData.MinDamage, WeaponData.MaxDamage);

            if (cancellationToken.IsCancellationRequested || !target.Health.IsAlive)
            {
                IsPendingToAttack = false;
                return;
            }
            
            target.Health.Effects.AddEffect(new InstantDamageEffect(damage));
            
            Attacked?.Invoke(this, new WeaponAttackedEventArgs(target, owner, this, damage));
            
            _stopwatch.Restart();
            IsPendingToAttack = false;
            
            Debug.Log($"Attacked with {damage} hp! Current target hp: {target.Health.Current}/{target.Health.Max}");
        }

        public event EventHandler<WeaponAttackedEventArgs> Attacked;
    }
}