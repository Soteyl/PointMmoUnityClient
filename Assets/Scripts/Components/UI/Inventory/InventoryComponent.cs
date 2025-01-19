using System.Linq;
using Business.Inventories;
using Components.Entity.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.UI.Inventory
{
    public class InventoryComponent: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private CharacterComponent _character;

        private IInventory Inventory => _character.Character.Inventory;
        
        private InventorySlotComponent[] _inventorySlots;

        public void Start()
        {
            _inventorySlots = GetComponentsInChildren<InventorySlotComponent>();
            
            for (var i = 0; i < _inventorySlots.Length; i++)
            {
                _inventorySlots[i].SetSlot(Inventory.ItemSlots.ElementAt(i));
            }
        }
    }
}