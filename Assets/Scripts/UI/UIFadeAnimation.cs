using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeAnimation : UIAnimation<CanvasGroup>
{
    [Header("Fade Animation")]
    [SerializeField] private float animationFinalAlpha = 0.0f;

    private float originalValue;
    private float currentValue;
    private float finalValue;

    protected override void SetupAnimation()
    {
        originalValue = component.alpha;
    }
    

    protected override void CalculateInTarget()
    {
        currentValue = component.alpha;
        finalValue = animationFinalAlpha;
    }

    protected override void CalculateOutTarget()
    {
        currentValue = component.alpha;
        finalValue = originalValue;
    }

    protected override void ApplyAnimation(float t)
    {
        component.alpha = Mathf.Lerp(currentValue, finalValue,  t);
    }
}
