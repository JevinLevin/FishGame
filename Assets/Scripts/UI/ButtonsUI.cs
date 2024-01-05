using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private UIFadeAnimation animationScript;
    public bool IsUIOpen { get; private set; }
    
    private Coroutine currentCoroutine;

    private void Awake()
    {
        animationScript = GetComponent<UIFadeAnimation>();
    }

    public void OnOpenUI()
    {
        IsUIOpen = true;
        canvasGroup.interactable = false;
        
        animationScript.PlayInAnimation();

    }
    public void OnCloseUI()
    {
        IsUIOpen = false;
        canvasGroup.interactable = true;

        animationScript.PlayOutAnimation();
    }
}
