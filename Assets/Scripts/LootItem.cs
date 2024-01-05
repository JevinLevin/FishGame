
using System;
using UnityEngine;

[Serializable]
public class LootItem
{
    [SerializeField] private LootScriptableObject itemObject;
    public LootScriptableObject ItemObject => itemObject;
    [SerializeField] private int itemCount;
    public int ItemCount => itemCount;

    // If lookup scriptable object if script only has item ID
    public static LootScriptableObject GetLootScriptableObject(string itemID)
    {
        foreach (LootScriptableObject loot in LootManager.lootScriptableObjects)
        {
            if (loot.LootID == itemID)
            {
                return loot;
            }
        }

        return null;
    }
}

