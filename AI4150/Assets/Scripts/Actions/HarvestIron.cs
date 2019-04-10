﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestIron : GOAPAction
{
    private float closeness = 5f;
    private new void Awake()
    {
        base.Awake();
        Cost = 50f;
    }

    public override void DoAction()
    {
        Debug.Log("you got some iron!");
    }

    public override bool CheckPreConditions(Dictionary<Condition, object> currentState)
    {
        return (currentState.ContainsKey(Condition.nearIron) && (float)currentState[Condition.nearIron] <= (float)preConditions[Condition.nearIron]);
    }

    protected override void AddPreConditions()
    {
        preConditions.Add(Condition.nearIron, closeness);
    }

    protected override void AddPostConditions()
    {
        postConditions.Add(Condition.hasIron, true);
    }
}
