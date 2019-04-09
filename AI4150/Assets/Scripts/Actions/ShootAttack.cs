using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : AbstractAttackAction , IWeaponAttack
{
    
    public ShootAttack(AbstractCreature target, int cost, int damage) : base(target, cost, damage)
    {
        preConditions.Add(Condition.hasRangedWeapon, true);
    }

    public void GetWeapon()
    {
        throw new System.NotImplementedException();
    }

    public void UseWeapon()
    {
        throw new System.NotImplementedException();
    }
}
