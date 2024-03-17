using System;
using Components.Entity;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Items.Loot
{
    public class DropItemOnEntityDeath: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private LootDropper _lootDropper;

        [OdinSerialize]
        private EntityComponent _entity;
        
        private void Awake()
        {
            _entity.Entity.Health.Died += OnDeath;
        }

        private void OnDeath(object sender, EventArgs e)
        {
            _lootDropper.Drop();
        }
    }
}