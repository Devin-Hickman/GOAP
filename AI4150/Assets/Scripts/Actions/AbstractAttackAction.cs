using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAttackAction : GOAPAction
{
    protected AbstractCreature target;
    protected int damage;

    private void Awake()
    {
        preConditions.Add(Condition.hasTarget, true);
    }

    public override IEnumerator DoAction(Dictionary<Condition, object> npcState)
    {
        base.DoAction(npcState);
        target.UnderAttack(damage);
        yield return null;
    }

    protected override void AddPostConditions()
    {
        postConditions.Add(Condition.damageTarget, true);
    }
}
