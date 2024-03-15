using System;

namespace Business.Inventories
{
    public class ItemSlot: IItemSlot
    {
        private ArgumentException NegativeCountException => new("Count cannot be negative");
        
        private IItemData _item;

        public IItemData Item
        {
            get => _item;
            set
            {
                _item = value;
                Count = 0; 
                IsEmpty = value == null;
            }
        }

        public bool IsEmpty { get; private set; } = true;
        
        public int Count { get; private set; }


        /// <param name="count">Positive count</param>
        /// <returns>Extra items, which cannot be added</returns>
        /// <exception cref="ArgumentException">Count cannot be negative</exception>
        public int Add(int count)
        {
            if (count < 0) throw NegativeCountException;
            
            int extra = count - _item.MaxCount + Count;
            Count = Math.Min(_item.MaxCount, Count + count);
            return Math.Max(extra, 0);
        }

        /// <param name="count">Positive count</param>
        /// <returns>Extra items, which cannot be removed</returns>
        /// <exception cref="ArgumentException">Count cannot be negative</exception>
        public int Remove(int count)
        {
            if (count < 0) throw NegativeCountException;
            
            int extra = Count - count;
            Count = Math.Max(0, extra);

            if (Count == 0)
            {
                Item = null;
                return extra * -1;
            }

            return 0;
        }

        public static ItemSlot Empty => new();
    }
}