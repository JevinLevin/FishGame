using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineTime : MonoBehaviour
{
    [Header("Assets")] 
    [SerializeField] private GameObject itemPanel;
    private List<UIItemPanel> itemPanels;

    [Header("Components")] 
    [SerializeField] private Transform itemsDisplay;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI invalidTimeText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Scrollbar scrollbar;

    [Header("Animations")]
    [SerializeField] private UIScaleAnimation scaleAnimation;
    [SerializeField] private UIFadeAnimation fadeAnimation;

    
    public void Start()
    {
        canvasGroup.alpha = 0.0f;
        transform.localScale = Vector3.zero;
        fadeAnimation.ReSetupAnimation();
        scaleAnimation.ReSetupAnimation();
    }

    private void OpenPopup()
    {
        UIBackgroundTint.FadeInBackgroundTint();
        
        gameObject.SetActive(true);
        
        scaleAnimation.PlayInAnimation();
        fadeAnimation.PlayInAnimation();
    }

    private void ClosePopup()
    {
        UIBackgroundTint.FadeOutBackgroundTint();

        
        scaleAnimation.PlayOutAnimation();
        fadeAnimation.PlayOutAnimation();
    }

    public void Display(TimeSpan timeSpan)
    {
        
        gameObject.SetActive(true);
        
        float baitTime = Player.BaitStorageValues[Player.GetUpgradeStage(Player.UpgradeIDs.BaitStorage)];
        float generateTime = Player.FishingSpeedValues[Player.GetUpgradeStage(Player.UpgradeIDs.FishingSpeed)];
        bool valid = timeSpan.TotalHours < baitTime;
        
        int numberOfRolls = (int)(valid ? timeSpan.TotalSeconds / generateTime : baitTime*60*60 / generateTime );

        // If nothing was generated then skip popup
        if (numberOfRolls <= 0)
            return;
        
        // Show time in text
        string totalTimeText = "";
        totalTimeText += timeSpan.Days > 0 ? "" + timeSpan.Days + "d " : "";
        totalTimeText += timeSpan.Hours > 0 ? "" + timeSpan.Hours + "h " : "";
        totalTimeText += timeSpan.Minutes > 0 ? "" + timeSpan.Minutes + "m " : "";
        totalTimeText += timeSpan.Seconds > 0 ? "" + timeSpan.Seconds + "s" : "";
        if (valid)
        {
            timeText.text = totalTimeText;
        }
        else
        {
            invalidTimeText.gameObject.SetActive(true);
            totalTimeText = "   (" + totalTimeText + ")";
            invalidTimeText.text = totalTimeText;
            timeText.text = "" + baitTime + "h";
        }
        
        // Generate loot and display
        Dictionary<LootScriptableObject, int> offlineLoot = LootManager.Instance.OfflineGenerate(numberOfRolls);
        foreach (KeyValuePair<LootScriptableObject, int> loot in offlineLoot.OrderBy(type => type.Key.LootType).ThenByDescending(loot => loot.Key.LootRarity).ThenBy(lootName => lootName.Key.LootName))
        {
            UIItemPanel tempPanel = Instantiate(itemPanel, itemsDisplay).GetComponent<UIItemPanel>();
            tempPanel.SetupPanel(loot.Key, loot.Value);
        }

        StartCoroutine(ResetScrollBar());
        
        OpenPopup();


    }
    
    private IEnumerator ResetScrollBar()
    {
        yield return null;
        yield return null;
        
        scrollbar.value = 0f;
    }

    public void Accept()
    {
        ClosePopup();
    }
}
