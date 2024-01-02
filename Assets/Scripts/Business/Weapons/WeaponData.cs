using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Business.Weapons
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Entities/Weapon")]
    public class WeaponData: SerializedScriptableObject
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
    }
}