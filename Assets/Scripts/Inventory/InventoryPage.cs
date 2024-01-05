using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private GameObject panelObject;
    [SerializeField] private GameObject panelParent;
    [SerializeField] private LootScriptableObject.LootTypes type;

    private Dictionary<LootScriptableObject.LootRarities, bool> rarityTags = new ();

    private List<InventoryPanel> panels = new();

    public bool PanelSet { get; private set; } = false;

    private void Start()
    {
        // Refreshes the panel when an item is collected in case an item is collected while a page is open
        GameManager.Instance.GetInventory().UpdateInventoryEvent += RefreshPanelData; 
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        
        RefreshPanelData();
    }

    public void ChangeTag(LootScriptableObject.LootRarities rarity)
    {
        // Flips the current state of the tag
        rarityTags[rarity] = !rarityTags[rarity];
        
        RefreshPanelList();
    }

    public void ResetTags()

    {
        foreach (var key in rarityTags.Keys.ToList())
        {
            rarityTags[key] = false;
        }
        
        RefreshPanelList();
    }
    private void RefreshPanelList()
    {
        foreach (InventoryPanel panel in panels)
        {
            // Set the game object active or inactive depending on if the tag is active
            panel.gameObject.SetActive(rarityTags[panel.LootObject.LootRarity]);
        }
        
        if (panels.Count(panel => panel.gameObject.activeSelf) != 0) return;
        
        // If there are no active tags, then interpret as all tags are active and set all panels to true
        foreach (InventoryPanel panel in panels)
        {
            panel.gameObject.SetActive(true);
        }
        
    }

    // Refreshes the data inside the panels in case the item data has changed
    private void RefreshPanelData()
    {
        foreach (InventoryPanel panel in panels)
        {
            panel.SetCount();
        }
    }

    // Generate all possible panels for page based on type and rarity
    public void SetPanels()
    {
        // Creates list of all items types that fit this page
        List<LootScriptableObject> lootList = new();
        switch (type)
        {
            case LootScriptableObject.LootTypes.Fish:
                lootList = LootManager.fishScriptableObjects;
                break;
            case LootScriptableObject.LootTypes.Material:
                lootList = LootManager.materialScriptableObjects;
                break;
            case LootScriptableObject.LootTypes.Junk:
                lootList = LootManager.junkScriptableObjects;
                break;
        }

        // Loops through all objects of type
        foreach (LootScriptableObject loot in lootList)
        {
            // Adds this rarity to possible tags
            rarityTags.TryAdd(loot.LootRarity, false);

            // Create new panel for item
            InventoryPanel newPanel = Instantiate(panelObject, panelParent.transform).GetComponent<InventoryPanel>();
            
            newPanel.SetupPanel(loot);
            
            panels.Add(newPanel);
            
        }
        
        RefreshPanelList();

        // This variable ensures this function is only ran once during runtime
        PanelSet = true;
    }
}
