using Business.Inventories;

namespace Business.Entities
{
    public class Character : Entity
    {
        public IInventory Inventory { get; } = new Inventory(24);
        
        public Character(): base()
        {
            
        }
    }
}
