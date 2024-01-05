using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    private Region[] regions;
    private Region selectedRegion;
    private Region selectedTime;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private UIFadeAnimation fadeAnimation;
    private float transitionLength;

    private void Start()
    {
        transitionLength = GameManager.Instance.BackgroundManager.TransitionDuration;

        regions = GetComponentsInChildren<Region>();
        foreach (Region region in regions)
        {
            if (region.region == GameManager.Instance.GetPlayer().CurrentRegion &&
                region.regionType == Region.RegionTypes.Region)
            {
                selectedRegion = region;
                selectedRegion.Activate();
            }
            if (region.time == GameManager.Instance.GetPlayer().CurrentTime &&
                region.regionType == Region.RegionTypes.Time)
            {
                selectedTime = region;
                selectedTime.Activate();
            }
        }
    }

    public void ClickRegion(Region region)
    {
        StartCoroutine(TransitionFade());
        
        region.Activate();

        switch (region.regionType)
        {
            case Region.RegionTypes.Region:
                if(selectedRegion) selectedRegion.Deactivate();
                selectedRegion = region;
                break;
            case Region.RegionTypes.Time:
                if(selectedTime) selectedTime.Deactivate();
                selectedTime = region;
                break;
        }
    }

    private IEnumerator TransitionFade()
    {
        FadeIn();

        float transitionDuration = 0.0f;

        while (transitionDuration < transitionLength)
        {
            transitionDuration += Time.deltaTime;
            yield return null;
        }
        
        FadeOut();
    }

    public void FadeIn()
    {
        fadeAnimation.PlayInAnimation();
        canvasGroup.interactable = false;
    }

    public void FadeOut()
    {
        fadeAnimation.PlayOutAnimation();
        canvasGroup.interactable = true;
    }
}
