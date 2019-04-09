using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    protected Dictionary<Condition, object> preConditions;
    protected Dictionary<Condition, object> postConditions;
    public Dictionary<Condition, object> PostConditions { get { return postConditions; } }
    public float Cost { get; set; }

    public Dictionary<Condition, object> GetPreConditionsToFulFill(Dictionary<Condition, object> currentState)
    {
        Dictionary<Condition, object> res = new Dictionary<Condition, object>();
        foreach (KeyValuePair<Condition, object> kvp in preConditions)
        {
            if(!currentState.ContainsKey(kvp.Key) || currentState[kvp.Key] != kvp.Value)
            {
                res.Add(kvp.Key, kvp.Value);
            }
        }
        return res;
    }

    public bool CheckPreConditions(Dictionary<Condition, object> currentState)
    {
        return GetPreConditionsToFulFill(currentState).Count == 0;
    }

    /// <summary>
    /// Takes in a state and applies the actions post conditions to it
    /// </summary>
    /// <param name="newState">Updated world state</param>
    public Dictionary<Condition, object> ApplyPostConditionsToState(Dictionary<Condition, object> curState)
    {
        Dictionary<Condition, object> newState = new Dictionary<Condition, object>(curState);
        foreach(KeyValuePair<Condition,object> kvp in postConditions)
        {
            if (newState.ContainsKey(kvp.Key))
            {
                newState[kvp.Key] = kvp.Value;
            }
            else
            {
                newState.Add(kvp.Key, kvp.Value);
            }
        }
        return newState;
    }

    public abstract void DoAction();
}
