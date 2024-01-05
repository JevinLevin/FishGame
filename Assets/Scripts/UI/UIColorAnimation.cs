using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorAnimation : UIAnimation<Image>
{
    [Header("Color Animation")]
    [SerializeField] private Color pulseColor;

    private Color originalValue;
    private Color currentValue;
    private Color finalValue;

    protected override void SetupAnimation()
    {
        originalValue = component.color;
    }
    

    protected override void CalculateInTarget()
    {
        currentValue = component.color;
        finalValue = pulseColor;
    }

    protected override void CalculateOutTarget()
    {
        currentValue = component.color;
        finalValue = originalValue;
    }

    protected override void ApplyAnimation(float t)
    {
        component.color = Color.Lerp(currentValue, finalValue,  t);
    }
}
