using Components.Interacting;
using Data.ScriptableObjects;
using Sirenix.Serialization;

namespace Components.Items
{
    public class LootItem: InteractableObject
    {
        [OdinSerialize]
        public ItemData ItemData { get; set; }
        
        [OdinSerialize]
        public int Count { get; set; }
        
        /// <param name="collectedCount">Collected count. If null, collect all</param>
        public void Collect(int? collectedCount = null)
        {
            if (collectedCount.HasValue)
            {
                Count -= collectedCount.Value;
                if (Count <= 0) Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}