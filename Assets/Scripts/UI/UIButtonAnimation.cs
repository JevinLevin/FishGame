using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool copyInteractable = true;
    [SerializeField] private UIAnimation<RectTransform> animationScript;
    
    private CanvasGroup canvasGroup;

    
    private void Awake()
    {
        if(!animationScript) animationScript = GetComponent<UIAnimation<RectTransform>>();
    }
    
    private void Start()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }
    
    // Checks if the mouse hovers or stops hovering the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (copyInteractable && canvasGroup && !canvasGroup.interactable) return;
        animationScript.PlayInAnimation();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animationScript.PlayOutAnimation();
    }
}
