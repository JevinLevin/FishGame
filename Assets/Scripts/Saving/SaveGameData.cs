using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Data class that stores all the data needing to be stored in a file
public class SaveGameData
{
    public DateTime lastLogin;
    
    public Dictionary<string, int> playerInventory;
    public Dictionary<Player.UpgradeIDs, int> playerUpgrades;

    public LootScriptableObject.LootRegions currentRegion;
    public LootScriptableObject.LootTimes currentTime;

    public SaveGameData()
    {
        playerInventory = new();
        playerUpgrades = new();

        currentRegion = LootScriptableObject.LootRegions.Ocean;
        currentTime = LootScriptableObject.LootTimes.Day;
        
        lastLogin = DateTime.Now;
    }

}
