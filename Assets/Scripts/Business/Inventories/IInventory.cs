using System.Collections.Generic;

namespace Business.Inventories
{
    public interface IInventory
    {
        /// <summary>
        /// Represents all item slots in the inventory. Can be empty (null).
        /// </summary>
        IReadOnlyCollection<IReadonlyItemSlot> ItemSlots { get; }
        
        int Size { get; }

        bool HasEmptySlots();

        bool HasSpaceFor(IItemData item, int count = 1);

        ItemCountChangeResult AddItem(IItemData item, int count = 1, bool failIfNotEnoughSpace = false);

        ItemCountChangeResult RemoveItem(IItemData item, int count = 1, bool failIfNotEnoughItems = false);
    }
}