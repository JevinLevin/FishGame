using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class UIItemPanel : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI itemCountText;
    [SerializeField] protected TextMeshProUGUI itemNameText;
    [SerializeField] protected Image panelBackground;
    [SerializeField] protected Image itemIcon;
    
    public LootScriptableObject LootObject { get; private set; }
    public int ItemCount { get; private set; }

    public void SetupPanel(LootScriptableObject loot, int count = 0)
    {
        LootObject = loot;
        ItemCount = count;
        
        SetIcon();
        SetName();
        SetCount();
        panelBackground.color = LootScriptableObject.GetRarityColor(loot.LootRarity);
    }

    // Changes the sprite used in panel
    protected virtual void SetIcon()
    {
        itemIcon.sprite = LootObject.LootSprite;
    }

    // Sets the name text to the item name
    protected virtual void SetName()
    {
        itemNameText.text = LootObject.LootName;
    }

    public virtual void SetCount()
    {
        itemCountText.text = ItemCount.ToString();
    }

    public virtual void SetCountColor(bool isValid)
    {
        itemCountText.color = isValid ? GameManager.Instance.YesColor : GameManager.Instance.NoColor;
    }
    
    public bool CheckCount()
    {
        SetCount();
        
        // Sets count color depending on if the player has the item and the right amount
        if (GameManager.Instance.GetInventory().PlayerInventory.ContainsKey(LootObject.LootID) &&
            GameManager.Instance.GetInventory().PlayerInventory[LootObject.LootID] >= ItemCount)
        {
            SetCountColor(true);
            return true;
        }

        SetCountColor(false);
        return false;
        
    }

    public void ChangeCount(int newCount)
    {
        ItemCount = newCount;
    }
}
