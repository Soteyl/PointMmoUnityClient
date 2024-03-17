using System.Collections.Generic;
using Data.ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class LootContainer : SerializedMonoBehaviour
{
    [OdinSerialize]
    public List<LootItem> LootItems { get; set; } = new();
}

public class LootItem
{
    [OdinSerialize]
    public ItemData ItemData { get; set; }
    
    [OdinSerialize]
    public int Count { get; set; }
}