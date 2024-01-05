using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private UpgradeScriptableObject upgradeScriptableObject;
    [Header("Components")]
    [SerializeField] private UpgradeInput upgradeInput;
    [SerializeField] private TextMeshProUGUI upgradeTitle;
    [SerializeField] private TextMeshProUGUI upgradeDesc;
    [SerializeField] private TextMeshProUGUI upgradeStageDesc;
    [SerializeField] private TextMeshProUGUI upgradeStageProgress;
    [SerializeField] private Image upgradeImage;

    private int stage;
    private int maxStage;

    private void Start()
    {
        // Refresh cost item counts whenever the inventory is altered
        GameManager.Instance.GetInventory().UpdateInventoryEvent += RefreshCounts;
    }

    public void Setup(UpgradeScriptableObject upgradeScriptableObject)
    {
        this.upgradeScriptableObject = upgradeScriptableObject;
        
        stage = Player.GetUpgradeStage(upgradeScriptableObject.UpgradeID);
        maxStage = upgradeScriptableObject.UpgradeStages.Length;
        
        // Sets the data specific to this upgrade stage
        SetupStage();

        // Sets upgrade data
        upgradeTitle.text = upgradeScriptableObject.UpgradeName;
        upgradeDesc.text = upgradeScriptableObject.UpgradeDesc;
        upgradeImage.sprite = upgradeScriptableObject.UpgradeIcon;
    }
    
    private void SetupStage()
    {

        // If maxed then nothing stage-wise needs to change
        if (CheckMax()) return;
        SetStageDesc();
        upgradeInput.Setup(upgradeScriptableObject.GetCost(stage));
    }
    

    // Refresh all the stage specific data
    private void RefreshStage()
    {
        
        // If not maxed, refresh the numbers
        if (CheckMax()) return;
        SetStageDesc();
        upgradeInput.UpdateItems(upgradeScriptableObject.GetCost(stage));
    }
    
    private void RefreshCounts()
    {
        if(CheckMax()) return;
        upgradeInput.Refresh();
    }
    

    private void SetStageDesc()
    {
        // Sets description for old stage, uses default desc if its the first stage
        string oldDesc = stage > 0 ? upgradeScriptableObject.GetStageDesc(stage - 1) : upgradeScriptableObject.DefaultStageDesc;
        upgradeStageDesc.text = oldDesc + " -> " + upgradeScriptableObject.GetStageDesc(stage);
        upgradeStageProgress.text = "" + stage + "/" + maxStage;
    }

    // Check if the current stage is the max stage
    private bool CheckMax()
    {
        if (stage < maxStage) return false;
        
        // If its the max stage, also change upgrade data accordingly 
        upgradeStageProgress.color = GameManager.Instance.YesColor;
        upgradeStageDesc.text = "MAX";
        upgradeStageProgress.text = "" + maxStage + "/" + maxStage;
        upgradeInput.Max();
        return true;
    }

    // When the player purchases an upgrade
    public void OnBuyUpgrade()
    {
        // Removes items used to purchased from inventory
        foreach (UIItemPanel input in upgradeInput.Panels)
            GameManager.Instance.GetInventory().RemoveFromDictionary(input.LootObject.LootID, input.ItemCount);
        
        Player.BuyUpgrade(upgradeScriptableObject.UpgradeID);
        
        stage++;
        
        // Updates to the next stage
        RefreshStage();
        
    }
}
