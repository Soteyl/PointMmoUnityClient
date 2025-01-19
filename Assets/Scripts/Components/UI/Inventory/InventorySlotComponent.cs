using Business.Inventories;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine.UI;

namespace Components.UI.Inventory
{
    public class InventorySlotComponent: SerializedMonoBehaviour
    {
        private IReadonlyItemSlot Slot { get; set; }

        [OdinSerialize]
        private Image _icon;
        
        [OdinSerialize]
        private TextMeshProUGUI _countText;
        
        public void SetSlot(IReadonlyItemSlot slot)
        {
            if (Slot is not null)
            {
                Slot.SlotChanged -= SlotOnSlotChanged;
            }
            Slot = slot;
            Slot.SlotChanged += SlotOnSlotChanged;
            
            SlotOnSlotChanged(this, new SlotChangedEventArgs(Slot));
        }

        private void SlotOnSlotChanged(object sender, SlotChangedEventArgs e)
        {
            _icon.enabled = !e.Slot.IsEmpty;
            if (e.Slot.IsEmpty) return;
            
            _icon.sprite = e.Slot.Item.Sprite;

            if (e.Slot.Item.MaxCount > 1 && e.Slot.Count > 1)
            {
                _countText.text = e.Slot.Count.ToString();
                _countText.enabled = true;
            }
            else _countText.enabled = false;
        }
    }
}