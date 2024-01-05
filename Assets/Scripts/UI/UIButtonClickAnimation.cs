using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UIAnimation<RectTransform> animationScript;
    
    private void Awake()
    {
        if(!animationScript) animationScript = GetComponent<UIAnimation<RectTransform>>();
    }

    private void Update()
    {
        //animationScript.PlayInAnimation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        animationScript.PlayInAnimation();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animationScript.PlayOutAnimation();
    }
}
