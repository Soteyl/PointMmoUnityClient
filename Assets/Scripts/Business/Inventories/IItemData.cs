using UnityEngine;

namespace Business.Inventories
{
    public interface IItemData
    {
        string Id { get; }
        
        int MaxCount { get; }

        Sprite Sprite { get; }
    }
}