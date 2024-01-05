using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCategoryButton : MonoBehaviour
{
    private UIMoveAnimation animationScript;
    private Button button;
    [SerializeField] Image image;
    private InventoryCategoryButton[] buttons;

    [SerializeField] private Color selectedColor;

    public bool IsClicked { get; private set; }

    private void Awake()
    {
        animationScript = GetComponent<UIMoveAnimation>();
        button = GetComponent<Button>();
    }

    private void Start()
    {

        buttons = transform.parent.GetComponentsInChildren<InventoryCategoryButton>();

    }

    public void OnButtonClicked(bool skipAnimation = false)
    {
        // Invert click state
        IsClicked = !IsClicked;

        // Pause the current move animation
        animationScript.PauseAnimation(IsClicked);
        
        button.interactable = !IsClicked;
        
        image.color = IsClicked ? selectedColor : Color.white;

        if (!skipAnimation) return;
        if (IsClicked)
        {
            // Jumps to the end of the animation so that it starts positioned low down
            animationScript.JumpToEnd();
            // Queues the out animation for once unpaused
            animationScript.PlayOutAnimation();
        }
        else
        {
            animationScript.JumpToStart();

        }


    }


}
