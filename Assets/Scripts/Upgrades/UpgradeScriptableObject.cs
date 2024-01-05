using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LootScriptableObject", menuName = "ScriptableObjects/Upgrade")]
public class UpgradeScriptableObject : ScriptableObject
{

    [SerializeField] private Player.UpgradeIDs upgradeID;
    [SerializeField] private string upgradeName;
    [SerializeField] private string upgradeDesc;
    [SerializeField] private Sprite upgradeIcon;
    [SerializeField] private string defaultStageDesc;
    [SerializeField] private UpgradeCost[] upgradeStages;
    [SerializeField] private int upgradeIndex;

    public Player.UpgradeIDs UpgradeID => upgradeID;
    public string UpgradeName => upgradeName;
    public string UpgradeDesc => upgradeDesc;
    public Sprite UpgradeIcon => upgradeIcon;
    public string DefaultStageDesc => defaultStageDesc;
    public UpgradeCost[] UpgradeStages => upgradeStages;
    public int UpgradeIndex => upgradeIndex;

    
    public LootItem[] GetCost(int level)
    {
        return upgradeStages[level].cost;
    }

    public string GetStageDesc(int level)
    {
        return upgradeStages[level].stageDesc;
    }
}

[Serializable]
public struct UpgradeCost
{
    public LootItem[] cost;
    public string stageDesc;
}
