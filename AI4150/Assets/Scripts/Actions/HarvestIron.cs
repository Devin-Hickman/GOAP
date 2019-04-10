using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestIron : GOAPAction
{
    private float closeness = 5f;
    private new void Awake()
    {
        base.Awake();
        cost = 50f;
    }

    public override IEnumerator DoAction()
    {
        Debug.Log("you got some iron!");
        yield return null;
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
