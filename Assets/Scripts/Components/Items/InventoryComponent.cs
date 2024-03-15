using System.Collections.Generic;
using System.Linq;
using Business.Inventories;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Components.Items
{
    public class InventoryComponent: SerializedMonoBehaviour
    {
        public Inventory Inventory { get; private set; }

        [OdinSerialize]
        private int _size = 40;

        // todo remove temporary debug
        [ShowInInspector]
        private IReadOnlyCollection<string> Items => Inventory?.ItemSlots.Select(x => x.Item?.Id).ToList();
        
        private void Awake()
        {
            Inventory = new Inventory(_size);
        }
    }
}