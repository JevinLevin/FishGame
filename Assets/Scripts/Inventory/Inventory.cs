using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISave
{
    public Dictionary<string, int> PlayerInventory { get; private set; }

    public event Action<LootScriptableObject, int> AddItemEvent;
    public event Action UpdateInventoryEvent;

    public event Action RemoveItemEvent;
    
    public void LoadData(SaveGameData saveGameData)
    {
        PlayerInventory = saveGameData.playerInventory;
    }

    public void SaveData(SaveGameData saveGameData)
    {
        saveGameData.playerInventory = PlayerInventory;
    }


    // Add item to inventory
    public void AddToDictionary(string loot)
    {
        if (PlayerInventory.ContainsKey(loot))
        {
            PlayerInventory[loot]++;
        }
        else
        {
            PlayerInventory.Add(loot, 1);
        }
        AddItemEvent?.Invoke(LootItem.GetLootScriptableObject(loot), 1);
        UpdateInventoryEvent?.Invoke();
    }

    // Remove item from inventory
    public void RemoveFromDictionary(string loot, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            if (PlayerInventory.ContainsKey(loot))
            {
                RemoveItemEvent?.Invoke();

                PlayerInventory[loot]--;
                if (PlayerInventory[loot] == 0)
                {
                    PlayerInventory.Remove(loot);
                }
            }
        }
    }
}

