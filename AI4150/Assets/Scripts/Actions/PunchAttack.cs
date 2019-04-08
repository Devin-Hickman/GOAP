using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : AbstractAttackAction
{
    
    PunchAttack(AbstractCreature target, int cost, int damage) : base(target, cost, damage)
    {
    }

    public override void DoAction()
    {
        target.UnderAttack(damage);
    }
}
