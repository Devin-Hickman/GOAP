using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherIron : GOAPAction
{
    private float closeness = 5f;
    private new void Awake()
    {
        base.Awake();
        preConditions.Add(Condition.nearIron, closeness);
    }

    public override void DoAction()
    {
        throw new System.NotImplementedException();
    }

    public override bool CheckPreConditions(Dictionary<Condition, object> currentState)
    {
        return (currentState.ContainsKey(Condition.nearIron) && (float)currentState[Condition.nearIron] <= (float)preConditions[Condition.nearIron]);
    }
}
