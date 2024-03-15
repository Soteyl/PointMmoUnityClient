using Business.Inventories;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Entities/InventoryItem")]
    public class ItemData: SerializedScriptableObject, IItemData
    {
        [OdinSerialize]
        public string Id { get; private set; }

        [OdinSerialize]
        public int MaxCount { get; private set; } = 50;
        
        [OdinSerialize]
        public Sprite Sprite { get; private set; }
    }
}