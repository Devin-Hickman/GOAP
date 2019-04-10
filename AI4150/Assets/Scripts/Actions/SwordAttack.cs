using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : AbstractAttackAction, IWeaponAttack
{
    private AbstractWeapon sword;

    private new void Awake()
    {
        base.Awake();
        cost = 50;
    }

    public void UseWeapon()
    {
        throw new System.NotImplementedException();
    }

    protected override void AddPreConditions()
    {
        preConditions.Add(Condition.hasSword, true);
    }

}
