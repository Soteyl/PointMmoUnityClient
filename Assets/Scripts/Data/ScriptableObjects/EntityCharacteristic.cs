using Business.Entities;
using Business.Weapons;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Entity", menuName = "Entities/Entity")]
    public class EntityCharacteristic: SerializedScriptableObject
    {
        [OdinSerialize]
        public float Speed { get; private set; }
        
        [OdinSerialize]
        public int MaxHealth { get; private set; }
        
        [OdinSerialize]
        public WeaponData DefaultWeaponData { get; private set; }

        public void Apply(Entity entity)
        {
            entity.Health.Max.UpdateDefaultValue(MaxHealth);
            entity.Health.Current = entity.Health.Max.GetValue();
            entity.Speed.UpdateDefaultValue(Speed);
            entity.EquipWeapon(new Weapon(DefaultWeaponData));
        }
    }
}