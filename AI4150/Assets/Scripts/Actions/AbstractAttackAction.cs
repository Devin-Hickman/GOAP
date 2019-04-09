using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttackAction : GOAPAction
{
    protected AbstractCreature target;
    protected int cost;
    protected int damage;

    public AbstractAttackAction(AbstractCreature t, int c, int d)
    {
        target = t;
        cost = c;
        damage = d;
        postConditions.Add(Condition.damageTarget, true);
    }

    public override void DoAction()
    {
        target.UnderAttack(damage);
    }
}
