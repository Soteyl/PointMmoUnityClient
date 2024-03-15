namespace Business.Inventories
{
    public interface IItemSlot: IReadonlyItemSlot
    {
        new IItemData Item { get; set; }
        
        new int Count { get; }
        
        new bool IsEmpty { get; }

        /// <returns>Extra items</returns>
        int Add(int count);
        
        /// <returns>Extra items</returns>
        int Remove(int count);
    }

    public interface IReadonlyItemSlot
    {
        IItemData Item { get; }
        
        int Count { get; }
        
        bool IsEmpty { get; }
    }
}