using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryPanel : UIItemPanel
{
    [SerializeField] private Shadow shadow;

    

    // Changes the count text and also updates sprite visibility depending on if they've already been collected
    public override void SetCount()
    {
        // Checks if the item is in the players inventory
        bool hasItem = GameManager.Instance.GetInventory().PlayerInventory.ContainsKey(LootObject.LootID);
        // If doesn't have item
        if (!hasItem)
        {
            // Blacks out item icon
            Color noItemColor = Color.black;
            noItemColor.a = 0.75f;
            itemIcon.color = noItemColor;
            shadow.enabled = false;
    
            // Removes text
            itemCountText.text = "";
            itemNameText.text = "";
            return;
        }

        // If player has item
        itemIcon.color = Color.white;
        itemCountText.text = GameManager.Instance.GetInventory().PlayerInventory[LootObject.LootID].ToString();
        SetName();
        shadow.enabled = true;

    }
}
