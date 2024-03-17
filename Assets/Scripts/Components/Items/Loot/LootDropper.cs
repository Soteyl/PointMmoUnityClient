using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Components.Items.Loot
{
    public class LootDropper: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private LootContainer _lootContainer;
        
        [OdinSerialize]
        private GameObject _lootPrefab;
        
        public void Drop()
        {
            foreach (var item in _lootContainer.LootItems)
            {
                var loot = Instantiate(_lootPrefab, transform.position, Quaternion.identity);
                var lootItem = loot.GetComponent<LootItem>();
                lootItem.ItemData = item.ItemData; 
                lootItem.Count = item.Count;
                
                loot.GetComponent<LootItemPhysics>().Throw();
            }

            _lootContainer.LootItems = new();
        }
    }
}