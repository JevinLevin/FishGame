using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISave
{
    public void LoadData(SaveGameData saveGameData)
    {
        playerUpgrades = saveGameData.playerUpgrades;

        CurrentRegion = saveGameData.currentRegion;
        CurrentTime = saveGameData.currentTime;

        lastLogin = saveGameData.lastLogin;
    }

    public void SaveData(SaveGameData saveGameData)
    {
        saveGameData.playerUpgrades = playerUpgrades;

        saveGameData.currentRegion = CurrentRegion;
        saveGameData.currentTime = CurrentTime;
    }

    public LootScriptableObject.LootRegions CurrentRegion { get; private set; }
    public LootScriptableObject.LootTimes CurrentTime { get; private set; }

    private DateTime lastLogin;

    #region upgradeValues

    public enum UpgradeIDs
    {
        ClickingPower,
        FishingSpeed,
        FishingLuck,
        BonusCatch,
        BaitStorage,
        JunkRate
    }
    
    // Stores all upgrades, with its ID and the stage
    private static Dictionary<UpgradeIDs, int> playerUpgrades = new();

    public static readonly float[] ClickingPowerValues =
        { 0.25f, 0.375f, 0.5f, 0.625f, 0.75f, 0.875f, 1.0f, 1.125f, 1.25f, 1.5f, 2.0f };
    public static readonly float[] FishingSpeedValues =
        { 10.0f, 9.5f, 9.0f, 8.5f, 8.0f, 7.5f, 7.0f, 6.0f, 5.0f, 4.0f, 2.0f };
    public static readonly Dictionary<LootScriptableObject.LootRarities, float>[] FishingLuckValues =
    {
        CreateRarityDictionary(1,1,1,1,1),
        CreateRarityDictionary(1,2,2,2,2),
        CreateRarityDictionary(1,2,4,4,4),
        CreateRarityDictionary(1,2,4,8,8),
        CreateRarityDictionary(1,2,4,8,16),
        CreateRarityDictionary(1,6,12,24,48)
    };
    private static Dictionary<LootScriptableObject.LootRarities, float> CreateRarityDictionary(float common, float rare, float epic, float legendary, float transcended)
    {
        return new Dictionary<LootScriptableObject.LootRarities, float>
        {
            { LootScriptableObject.LootRarities.Common, common },
            { LootScriptableObject.LootRarities.Rare, rare },
            { LootScriptableObject.LootRarities.Epic, epic },
            { LootScriptableObject.LootRarities.Legendary, legendary },
            { LootScriptableObject.LootRarities.Transcended, transcended }
        };
    }
    public static readonly float[] BonusCatchValues =
        { 0.0f, 0.10f, 0.25f, 0.5f, 1.0f, 2.5f};
    public static readonly float[] BaitStorageValues =
        { 2.0f, 4.0f, 8.0f, 24.0f};
    public static readonly float[] JunkRateValues =
        { 1.0f, 0.75f, 0.5f, 0.2f};
    #endregion

    private void Start()
    {
        foreach (UpgradeScriptableObject upgrade in LootManager.upgradeScriptableObjects)
        {
            playerUpgrades.TryAdd(upgrade.UpgradeID, 0);
        }
        
        GameManager.Instance.BackgroundManager.SetRegion(CurrentRegion);
        GameManager.Instance.BackgroundManager.SetTime(CurrentTime);
        
        OfflineTime();
    }

    private void OfflineTime()
    {
        TimeSpan time = DateTime.Now - lastLogin;

        //print($"{time.Days} Days {time.Hours} Hours {time.Minutes} Minutes {time.Seconds} Seconds ago");
        
        GameManager.Instance.DisplayOfflineTime(time);
}

    public static void BuyUpgrade(UpgradeIDs upgradeID)
    {
        playerUpgrades[upgradeID]++;
    }

    public static int GetUpgradeStage(UpgradeIDs upgradeID)
    {
        return playerUpgrades[upgradeID];
    }

    // Why cant i use an enum as an event parameter its so stupid
    public void ChangeRegionOcean()
    {
        ChangeRegion(LootScriptableObject.LootRegions.Ocean);
    }
    public void ChangeRegionRiver()
    {
        ChangeRegion(LootScriptableObject.LootRegions.River);
    }
    public void ChangeRegionPond()
    {
        ChangeRegion(LootScriptableObject.LootRegions.Pond);
    }
    
    public void ChangeTimeDay()
    {
        ChangeTime(LootScriptableObject.LootTimes.Day);
    }
    public void ChangeTimeNight()
    {
        ChangeTime(LootScriptableObject.LootTimes.Night);
    }
    
    private void ChangeRegion(LootScriptableObject.LootRegions region)
    {
        CurrentRegion = region;
        GameManager.Instance.BackgroundManager.ChangeRegion(region);
    }
    
    private void ChangeTime(LootScriptableObject.LootTimes time)
    {
        CurrentTime = time;
        GameManager.Instance.BackgroundManager.ChangeTime(time);
    }
}
