using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Bundle : MonoBehaviour
{
    public BundleScriptableObject BundleScriptableObject { get; private set; }

    [Header("Assets")]
    [SerializeField] private GameObject panelObject;

    [Header("Objects")]
    [SerializeField] private GameObject inputObject;
    [SerializeField] private GameObject outputObject;

    [Header("Components")] 
    [SerializeField] private TextMeshProUGUI bundleName;
    [SerializeField] private Image bundleBackground;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    private List<BundlePanel> inputs = new();
    private List<BundlePanel> outputs = new();
    

    public void SetBundle(BundleScriptableObject bundle)
    {
        BundleScriptableObject = bundle;
        
        bundleName.text = BundleScriptableObject.BundleName;
        
        foreach (LootItem input in BundleScriptableObject.Input)
        {
            CreatePanel(input, inputObject, inputs);
        }
        foreach (LootItem output in BundleScriptableObject.Output)
        {
            CreatePanel(output, outputObject, outputs);
        }
        CheckCounts();
        bundleBackground.color = LootScriptableObject.GetRarityColor(bundle.BundleRarity);

    }

    private void CreatePanel(LootItem item, GameObject parent, List<BundlePanel> panelList)
    {
        BundlePanel bundlePanel = Instantiate(panelObject.transform, parent.transform).GetComponent<BundlePanel>();
        bundlePanel.SetupPanel(item.ItemObject, item.ItemCount);
        panelList.Add(bundlePanel);
    }

    public void CheckCounts()
    {
        Color tempGreen = GameManager.Instance.YesColor;
        bool canBuy = true;
        
        foreach (BundlePanel input in inputs)
        {
            if (!input.CheckCount())
                canBuy = false;

        }
        foreach (BundlePanel output in outputs)
        {
            output.SetCountColor(canBuy);
        }

        button.interactable = canBuy;
        buttonImage.color = canBuy ? tempGreen : new Color(tempGreen.r - 0.5f, tempGreen.g - 0.5f, tempGreen.b - 0.5f, 0.75f);
    }

    public void OnBuyBundle()
    {
        // Removes items used to purchased
        foreach (BundlePanel panel in inputs)
            GameManager.Instance.GetInventory().RemoveFromDictionary(panel.LootObject.LootID, panel.ItemCount);
        
        // Adds items that were purchased
        foreach (BundlePanel panel in outputs)
            GameManager.Instance.GetInventory().AddToDictionary(panel.LootObject.LootID);
        
        CheckCounts();
    }

}


