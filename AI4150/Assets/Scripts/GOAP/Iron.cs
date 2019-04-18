using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : GOAPInteractable
{
    private void Awake()
    {
        c = Condition.nearIron;
    }

    private void OnMouseEnter()
    {
        Tooltip.DisplayTooltip("Iron");
    }

    private void OnMouseExit()
    {
        Tooltip.HideTooltip();
    }
}
