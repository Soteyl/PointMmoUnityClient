namespace Business.Inventories
{
    public interface IItemData
    {
        string Id { get; }
        
        int MaxCount { get; }
    }
}