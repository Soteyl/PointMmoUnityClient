using System.Collections.Generic;
using System.Linq;

namespace Business.Inventories
{
    public class Inventory: IInventory
    {
        private readonly ItemSlot[] _items;

        public Inventory(int size = 40)
        {
            Size = size;
            _items = new ItemSlot[size].Select(_ => new ItemSlot()).ToArray();
        }

        public IReadOnlyCollection<IReadonlyItemSlot> ItemSlots => _items;

        public int Size { get; }

        public bool HasEmptySlots()
        {
            return _items.Any(x => x.IsEmpty);
        }

        public bool HasSpaceFor(IItemData item, int count = 1)
        {
            return _items.Where(x => !x.IsEmpty && x.Item.Id.Equals(item.Id)).Sum(x => x.Item.MaxCount - x.Count) 
                   + _items.Count(x => x.IsEmpty) * item.MaxCount >= count;
        }

        public ItemCountChangeResult AddItem(IItemData item, int count = 1, bool failIfNotEnoughSpace = false)
        {
            if (failIfNotEnoughSpace && !HasSpaceFor(item, count))
                return new ItemCountChangeResult {ExtraItems = count, ItemData = item};
            
            count = FillSlots(_items.Where(x => !x.IsEmpty && x.Item.Id.Equals(item.Id)), item, count);
            if (count > 0)
                count = FillSlots(_items.Where(x => x.IsEmpty), item, count);
            
            return new ItemCountChangeResult {ExtraItems = count, ItemData = item};
        }

        public ItemCountChangeResult RemoveItem(IItemData item, int count = 1, bool failIfNotEnoughItems = false)
        {
            if (failIfNotEnoughItems && _items.Where(x => !x.IsEmpty && x.Item.Id.Equals(item.Id)).Sum(x => x.Count) < count)
                return new ItemCountChangeResult {ExtraItems = count, ItemData = item};
            
            foreach (var slot in _items.Where(x => !x.IsEmpty && x.Item.Id.Equals(item.Id)))
            {
                count = slot.Remove(count);

                if (count == 0) break;
            }
            return new ItemCountChangeResult() { ExtraItems = count, ItemData = item };
        }

        private int FillSlots(IEnumerable<IItemSlot> slots, IItemData item, int count)
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty)
                    slot.Item = item;
                
                count = slot.Add(count);
                if (count == 0) return 0;
            }

            return count;
        }
    }
}