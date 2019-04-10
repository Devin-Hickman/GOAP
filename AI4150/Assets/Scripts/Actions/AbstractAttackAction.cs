using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttackAction : GOAPAction
{
    protected AbstractCreature target;
    protected int cost;
    protected int damage;

    private void Awake()
    {
        preConditions.Add(Condition.hasTarget, true);
    }

    public override void DoAction()
    {
        target.UnderAttack(damage);
    }

    protected override void AddPostConditions()
    {
        postConditions.Add(Condition.damageTarget, true);
    }
}
