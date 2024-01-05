using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryHeader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Color defaultTextColor;
    [SerializeField] private Color inactiveTextColor;

    private bool active = false;

    private void Start()
    {
        SetTextColor();
    }
    
    public void ClickHeader()
    {
        // Flips active bool and changes text color
        // The active bool on this class should match whatever tags are active in the page class
        active = !active;
        SetTextColor();
    }

    // Incase the header needs to be forced off
    public void Deactivate()
    {
        active = false;
        SetTextColor();
    }

    private void SetTextColor()
    {
        text.color = active ? defaultTextColor : inactiveTextColor;
    }
}
