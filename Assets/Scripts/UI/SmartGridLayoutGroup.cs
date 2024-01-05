using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmartHorizontalLayoutGroup : GridLayoutGroup
{
    
    public override void CalculateLayoutInputVertical()
    {
        startCorner = Corner.UpperLeft;
        startAxis = Axis.Horizontal;
        constraint = Constraint.Flexible;
        
        
        base.CalculateLayoutInputHorizontal();
        
        int childCount = 0;
        foreach(Transform child in transform)
        {
            if(child.gameObject.activeInHierarchy)
                childCount++;
        }
        
        float containerWidth = GetComponent<RectTransform>().rect.width - spacing.x*2*childCount - padding.horizontal;
        float containerHeight = GetComponent<RectTransform>().rect.height - spacing.y*2 - padding.vertical;

        float panelSize = Mathf.Min(containerWidth / childCount, containerHeight );
        
        cellSize = new Vector2(panelSize, panelSize); 
    }
}
