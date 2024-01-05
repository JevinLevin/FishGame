using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("Assets")] 
    [SerializeField] private GameObject upgradeObject;

    [Header("Objects")] 
    [SerializeField] private GameObject upgradePanel;

    [Header("Components")] 
    [SerializeField] private Scrollbar scrollbar;
    
    private List<Upgrade> upgrades = new();
    
    private void Start()
    {
        SetUpgrades();
    }

    private void SetUpgrades()
    {
        for (int i = 0; i < LootManager.upgradeScriptableObjects.Count; i++)
        {

            Upgrade newUpgrade = Instantiate(upgradeObject, upgradePanel.transform).GetComponent<Upgrade>();
            newUpgrade.Setup(LootManager.upgradeScriptableObjects[i]);
            upgrades.Add(newUpgrade);
        }
    }

    public void OnOpenUI()
    {
        StartCoroutine(UIPanel.ResetScrollBar(scrollbar));
    }
    
    
}
