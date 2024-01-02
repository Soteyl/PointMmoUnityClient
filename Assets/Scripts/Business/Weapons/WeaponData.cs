using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Business.Entities;
using Business.HealthPoints;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Business.Weapons
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Entities/Weapon")]
    public class WeaponData: SerializedScriptableObject, IWeapon
    {
        private const int CoyoteTimeMilliseconds = 200;
        
        [OdinSerialize]
        public float Distance { get; set; }
        
        [OdinSerialize]
        public float Speed { get; set; }
        
        [OdinSerialize]
        public float StayTime { get; set; }
        
        [OdinSerialize]
        public int MinDamage { get; set; }
        
        [OdinSerialize]
        public int MaxDamage { get; set; }
        
        public bool IsReloading => _stopwatch.ElapsedMilliseconds < Delay;
        
        public bool IsPendingToAttack { get; private set; }
        
        private float Delay => 1000 / Speed;

        private Stopwatch _stopwatch = new();

        private Random _random = new();

        public WeaponData()
        {
            _stopwatch.Start();
        }

        public virtual async Task AttackAsync(Entity owner, Entity target, CancellationToken cancellationToken = default)
        {
            if (IsPendingToAttack || _stopwatch.ElapsedMilliseconds < Delay - CoyoteTimeMilliseconds)
                return;
            
            if (IsReloading)
            {
                IsPendingToAttack = true;
                await Task.Delay((int)Mathf.Round(Delay - _stopwatch.ElapsedMilliseconds), cancellationToken);
            }

            int damage = _random.Next(MinDamage, MaxDamage);

            if (cancellationToken.IsCancellationRequested)
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