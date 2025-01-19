namespace Business.Inventories
{
    public class SlotChangedEventArgs
    {
        public IReadonlyItemSlot Slot { get; }
        
        public SlotChangedEventArgs(IReadonlyItemSlot slot)
        {
            Slot = slot;
        }
    }
}