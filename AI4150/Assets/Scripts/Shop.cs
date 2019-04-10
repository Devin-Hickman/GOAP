using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : GOAPInteractable
{

    private void Awake()
    {
        c = Condition.nearShop;
    }
}
