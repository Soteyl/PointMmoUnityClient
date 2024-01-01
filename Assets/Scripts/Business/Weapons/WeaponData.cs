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

        private Stopwatch _stopwatch = new();

        private Random _random = new();

        public WeaponData()
        {
            _stopwatch.Start();
        }

        public virtual async Task AttackAsync(Entity owner, Entity target, CancellationToken cancellationToken = default)
        {
            float delay = 1000 / Speed;
            if (_stopwatch.ElapsedMilliseconds < delay)
            {
                await Task.Delay((int)Mathf.Round(delay - _stopwatch.ElapsedMilliseconds), cancellationToken);
            }

            int damage = _random.Next(MinDamage, MaxDamage);

            if (cancellationToken.IsCancellationRequested)
                return;
            
            target.Health.Effects.AddEffect(new InstantDamageEffect(damage));
            
            Attacked?.Invoke(this, new WeaponAttackedEventArgs(target, owner, this, damage));
            
            Debug.Log("Attacked!");
        }

        public event EventHandler<WeaponAttackedEventArgs> Attacked;
    }
}