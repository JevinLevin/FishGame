using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BundleUI : MonoBehaviour
{
    [Header("Assets")] 
    [SerializeField] private GameObject bundleObject;
    [SerializeField] private GameObject bundlePair;

    [Header("Objects")] 
    [SerializeField] private GameObject bundlePanel;

    [Header("Components")] 
    [SerializeField] private Scrollbar scrollbar;
    

    private List<Bundle> bundles = new();
    private Transform currentPair;

    private void Start()
    {
        SetBundles();
    }
    
    public void OnOpenUI()
    {
        CheckCounts();
        StartCoroutine(UIPanel.ResetScrollBar(scrollbar));
    }


    private void SetBundles()
    {
        for (int i = 0; i < LootManager.bundleScriptableObjects.Count; i++)
        {
            // If number is even, create a new pair
            if (i % 2 == 0)
                currentPair = Instantiate(bundlePair.transform, bundlePanel.transform);

            Bundle newBundle = Instantiate(bundleObject.transform, currentPair).GetComponent<Bundle>();
            newBundle.SetBundle(LootManager.bundleScriptableObjects[i]);
            bundles.Add(newBundle);
        }
    }

    private void CheckCounts()
    {
        foreach (Bundle bundle in bundles)
        {
            bundle.CheckCounts();
        }
    }
}
