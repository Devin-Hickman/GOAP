using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : AbstractAttackAction, IWeaponAttack
{
    private AbstractWeapon sword;

    public SwordAttack(AbstractCreature target, int cost, int damage) : base(target, cost, damage)
    {
        preConditions.Add(Condition.hasSword, true);
    }

    public override void DoAction()
    {
        target.UnderAttack(sword.Damage);
    }

    public void UseWeapon()
    {
        throw new System.NotImplementedException();
    }

}
