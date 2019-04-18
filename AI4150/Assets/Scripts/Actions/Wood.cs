using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : GOAPInteractable
{
    private void Awake()
    {
        c = Condition.nearWood;
    }

    private void OnMouseEnter()
    {
        Tooltip.DisplayTooltip("Wood");
    }

    private void OnMouseExit()
    {
        Tooltip.HideTooltip();
    }
}
