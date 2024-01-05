using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveAnimation : UIAnimation<RectTransform>
{
    [Header("Move Animation")]
    [SerializeField] private Vector2 animationMoveDistance;
    public Vector2 MoveDistance => animationMoveDistance;


    private Vector3 originalPos;
    private Vector3 currentPos;
    private Vector3 finalPos;

    protected override void SetupAnimation()
    {
        originalPos = component.anchoredPosition;
    }
    

    protected override void CalculateInTarget()
    {
        currentPos = component.anchoredPosition;
        finalPos = originalPos + (Vector3)animationMoveDistance;
    }

    protected override void CalculateOutTarget()
    {
        currentPos = component.anchoredPosition;
        finalPos = originalPos;
    }

    protected override void ApplyAnimation(float t)
    {
        component.anchoredPosition = Vector3.Lerp(currentPos, finalPos,  t);
    }
}
