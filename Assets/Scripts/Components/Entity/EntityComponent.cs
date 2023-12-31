﻿using Data.ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Entity
{
    public abstract class EntityComponent: SerializedMonoBehaviour
    {
        public Business.Entities.Entity Entity { get; }

        [ShowInInspector]
        public float CurrentHealth => Entity.Health.Current;

        [OdinSerialize]
        private EntityCharacteristic Characteristic { get; set; }

        protected EntityComponent(Business.Entities.Entity entity)
        {
            Entity = entity;
        }

        private void Start()
        {
            Characteristic?.Apply(Entity);
        }
    }
}