using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    protected Dictionary<Condition, bool> preConditions = new Dictionary<Condition, bool>();
    protected Dictionary<Condition, bool> postConditions = new Dictionary<Condition, bool>();
    public Dictionary<Condition, bool> PostConditions { get { return postConditions; } }
    public float Cost { get; set; }

    protected void Awake()
    {
        preConditions = new Dictionary<Condition, bool>();
        postConditions = new Dictionary<Condition, bool>();
    }

    public Dictionary<Condition, bool> GetPreConditionsToFulFill(Dictionary<Condition, bool> currentState)
    {
        Dictionary<Condition, bool> res = new Dictionary<Condition, bool>();
        foreach (KeyValuePair<Condition, bool> kvp in preConditions)
        {
            if(!currentState.ContainsKey(kvp.Key) || currentState[kvp.Key] != kvp.Value)
            {
                res.Add(kvp.Key, kvp.Value);
            }
        }
        return res;
    }

    public bool CheckPreConditions(Dictionary<Condition, bool> currentState)
    {
        return GetPreConditionsToFulFill(currentState).Count == 0;
    }

    /// <summary>
    /// Takes in a state and applies the actions post conditions to it
    /// </summary>
    /// <param name="newState">Updated world state</param>
    public Dictionary<Condition, bool> ApplyPostConditionsToState(Dictionary<Condition, bool> curState)
    {
        Dictionary<Condition, bool> newState = new Dictionary<Condition, bool>(curState);
        foreach(KeyValuePair<Condition, bool> kvp in postConditions)
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
