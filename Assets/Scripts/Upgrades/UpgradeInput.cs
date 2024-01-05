using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInput : MonoBehaviour
{
    [SerializeField] private GameObject costPanel;
    [SerializeField] private GameObject itemPanel;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText;
    private Color disabledButton;
    private List<UIItemPanel> panels = new();
    public List<UIItemPanel> Panels => panels;

    private void Awake()
    {
        Color tempGreen = GameManager.Instance.YesColor;
        disabledButton = new Color(tempGreen.r - 0.5f, tempGreen.g - 0.5f, tempGreen.b - 0.5f, 0.75f);
    }

    // Create the cost items
    public void Setup(LootItem[] items)
    {
        foreach (LootItem item in items)
        {
            panels.Add(CreateItem(item));
        }

        SetCounts();
    }

    public void Refresh()
    {
        // Refreshes all counts
        SetCounts();
    }

    // Clear all item cost panels from the box
    private void Clear()
    {
        foreach (UIItemPanel panel in panels)
        {
            Destroy(panel.gameObject);
        }
        panels.Clear();
    }

    // Refreshes all the cost items and add any ones missing
    public void UpdateItems(LootItem[] items)
    {
        // Creates any new items and update current item counts
        foreach (LootItem item in items)
        {
            bool hasPanel = false;
            foreach (UIItemPanel panel in panels)
            {
                // If the item is missing from the panels its new
                if (panel.LootObject.LootID == item.ItemObject.LootID)
                {
                    hasPanel = true;
                    
                    // Updates input count
                    panel.ChangeCount(item.ItemCount);
                }

            }
            if(!hasPanel)
                panels.Add(CreateItem(item));
        }
        
        Refresh();
    }

    // Creates a new cost item
    private UIItemPanel CreateItem(LootItem item)
    {
        UIItemPanel newPanel = Instantiate(itemPanel, costPanel.transform).GetComponent<UIItemPanel>();
            
        newPanel.SetupPanel(item.ItemObject, item.ItemCount);

        return newPanel;
    }
    
    // Checks if the player can afford the upgrade
    private void SetCounts()
    {
        bool canBuy = true;

        foreach (UIItemPanel panel in panels)
        {
            if(!panel.CheckCount())
                canBuy = false;
        }
        
        button.interactable = canBuy;
        buttonImage.color = canBuy ? GameManager.Instance.YesColor : disabledButton;
    }

    // If the upgrade is maxed
    public void Max()
    {
        button.interactable = false;
        buttonText.text = "MAX";
        buttonImage.color = disabledButton;
        Clear();
    }
}
