using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    [SerializeField] private UIFadeAnimation fadeAnimation;
    [SerializeField] private Button button;
    public enum RegionTypes
    {
        Region,
        Time
    }

    public LootScriptableObject.LootRegions region;
    public LootScriptableObject.LootTimes time;

    public RegionTypes regionType;
    public void Activate()
    {
        fadeAnimation.PlayInAnimation();
        button.interactable = false;
    }

    public void Deactivate()
    {
        fadeAnimation.PlayOutAnimation();
        button.interactable = true;
    }
}
