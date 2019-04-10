using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : AbstractAttackAction
{
    private new void Awake()
    {
        base.Awake();
        cost = 100f;
    }

    protected override void AddPreConditions()
    {

    }
}
