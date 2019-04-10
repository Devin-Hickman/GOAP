using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : AbstractAttackAction
{
    private new void Awake()
    {
        base.Awake();
        Cost = 100f;
    }

    protected override void AddPreConditions()
    {

    }
}
