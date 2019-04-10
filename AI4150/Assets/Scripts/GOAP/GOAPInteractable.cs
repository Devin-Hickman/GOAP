using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPInteractable : MonoBehaviour
{
    //Set In child Awake
    protected Condition c;

    public virtual Dictionary<Condition, object> ApplyPostConditions(Dictionary<Condition, object> curState, float dist)
    {
        Dictionary<Condition, object> newState = new Dictionary<Condition, object>(curState);
        if (newState.ContainsKey(c))
        {
            newState[c] = dist;
        }
        else
        {
            newState.Add(c, dist);
        }
        return newState;
    }
}
