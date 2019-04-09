using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : AbstractAttackAction, IWeaponAttack
{
    private AbstractWeapon sword;

    private new void Awake()
    {
        base.Awake();
        preConditions.Add(Condition.hasSword, true);
    }

    public SwordAttack(AbstractCreature target, int cost, int damage) : base(target, cost, damage)
    {
        preConditions.Add(Condition.hasSword, true);
    }

    public void UseWeapon()
    {
        throw new System.NotImplementedException();
    }

}
