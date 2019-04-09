using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iron : GOAPInteractable
{
    public override Dictionary<Condition, object> ApplyPostConditions(Dictionary<Condition, object> curState, float dist)
    {
        Dictionary<Condition, object> newState = new Dictionary<Condition, object>(curState);
        if (newState.ContainsKey(Condition.nearIron))
        {
            newState[Condition.nearIron] = dist;
        }
        else
        {
            newState.Add(Condition.nearIron, dist);
        }
        return newState;
    }
}
