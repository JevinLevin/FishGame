using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryCategory : MonoBehaviour
{
    private InventoryPage page;
    
    [FormerlySerializedAs("header")] [SerializeField] private GameObject headers;
    private InventoryHeader[] headerClasses;
    [SerializeField] private InventoryCategoryButton button;
    private Scrollbar scrollBar;

    private void Awake()
    {
        // Grab all pages in category
        page = GetComponentInChildren<InventoryPage>(true);
        headerClasses = headers.GetComponentsInChildren<InventoryHeader>();

        scrollBar = GetComponentInChildren<Scrollbar>(true);
    }

    // Start displaying this category and page
    public void ActivateCategory()
    {
        headers.SetActive(true);
        DeactivateHeaders();
        page.ResetTags();
        StartCoroutine(UIPanel.ResetScrollBar(scrollBar));

        ActivatePage();

    }

    // Stops displaying category
    public void DeactivateCategory()
    {
        headers.SetActive(false);

        DeactivatePage();
    }

    // If a new page is clicked
    public void ClickTag(int rarityType)
    {
        page.ChangeTag((LootScriptableObject.LootRarities)rarityType);
        //DeactivatePage(selectedPage);
        //ActivatePage(page);
    }
    
    // Display new page
    private void ActivatePage()
    {
        page.Activate();

        // If the page has already been setup, dont set it up again
        if (page.PanelSet) return;
        page.SetPanels();
    }

    // Stop displaying page
    private void DeactivatePage()
    {
        if(page)page.gameObject.SetActive(false);
    }

    public InventoryCategoryButton GetButton()
    {
        return button;
    }

    public void DeactivateHeaders()
    {
        foreach (InventoryHeader header in headerClasses)
        {
            header.Deactivate();
        }
    }
}
