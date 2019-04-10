using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : AbstractAttackAction , IWeaponAttack
{
    private new void Awake()
    {
        base.Awake();
        cost = 40;
    }


    public void GetWeapon()
    {
        throw new System.NotImplementedException();
    }

    public void UseWeapon()
    {
        throw new System.NotImplementedException();
    }

    protected override void AddPreConditions()
    {
        preConditions.Add(Condition.hasRangedWeapon, true);
    }

}
