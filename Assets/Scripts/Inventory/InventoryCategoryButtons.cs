using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCategoryButtons : MonoBehaviour
{

    private InventoryCategoryButton selectedButton;

    public void ClickButton(InventoryCategoryButton button, bool skipAnimation)
    {
        if(selectedButton)selectedButton.OnButtonClicked(skipAnimation);

        selectedButton = button;
        
        button.OnButtonClicked(skipAnimation);
    }

}
