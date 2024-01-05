using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScaleAnimation : UIAnimation<RectTransform>
{
    [Header("Scale Animation")]
    [SerializeField] private float animationScale = 1.06f;

    private Vector3 originalScale;
    private Vector3 currentScale;
    private Vector3 finalScale;

    
    
    protected override void SetupAnimation()
    {
        originalScale = component.transform.localScale;
    }
    

    protected override void CalculateInTarget()
    {
        currentScale = component.transform.localScale;
        
        finalScale = originalScale * animationScale;
        
        // If the scale is set to 0 at the start
        if (originalScale == Vector3.zero)
            finalScale = Vector3.one * animationScale;
    }

    protected override void CalculateOutTarget()
    {
        currentScale = component.transform.localScale;
        finalScale = originalScale;
    }

    protected override void ApplyAnimation(float t)
    {
        component.transform.localScale = Vector3.Lerp(currentScale, finalScale,  t);
    }
}
