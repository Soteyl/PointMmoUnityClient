namespace Business.Inventories
{
    public class ItemCountChangeResult
    {
        public IItemData ItemData { get; set; }
        
        public int ExtraItems { get; set; }
    }
}