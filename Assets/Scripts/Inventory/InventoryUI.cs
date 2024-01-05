using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private InventoryCategory[] categories;
    [SerializeField] private InventoryCategoryButtons categoryButtonManager;
    
    
    private InventoryCategory selectedCategory;

    private void Awake()
    {
        // Default to first category
        selectedCategory = categories[0];
    }

    // When a category header is clicked
    public void OnClickCategory(InventoryCategory category)
    {
        ClickCategory(category);
        ClickCategoryButton(category);

    }

    // Ran when a new category is selected
    private void ClickCategory(InventoryCategory category)
    {
        // Switch category
        DeactivateCategory(selectedCategory);
        ActivateCategory(category);

        selectedCategory = category;
    }

    // Runs as the category button for the clicked category
    private void ClickCategoryButton(InventoryCategory category, bool skipAnimation = false)
    {
        categoryButtonManager.ClickButton(category.GetButton(), skipAnimation);
    }

    // Removes the category from display
    private void DeactivateCategory(InventoryCategory category)
    {

        category.DeactivateCategory();
        
        category.gameObject.SetActive(false);
    }
    
    // Displays passed category
    private void ActivateCategory(InventoryCategory category)
    {
        // This is now the displayed category
        selectedCategory = category;
        
        category.gameObject.SetActive(true);

        category.ActivateCategory();

    }

    // Sets to default categories
    public void ResetCategories()
    {
        ClickCategory(categories[0]);
        ClickCategoryButton(selectedCategory, true);
    }
}
