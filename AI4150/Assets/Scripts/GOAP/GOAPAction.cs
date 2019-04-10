using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    protected Dictionary<Condition, object> preConditions = new Dictionary<Condition, object>();
    protected Dictionary<Condition, object> postConditions = new Dictionary<Condition, object>();
    public Dictionary<Condition, object> PostConditions { get { return postConditions; } }
    public float cost;

    protected void Awake()
    {
        preConditions = new Dictionary<Condition, object>();
        postConditions = new Dictionary<Condition, object>();
        AddPostConditions();
        AddPreConditions();
    }

    public Dictionary<Condition, object> GetPreConditionsToFulFill(Dictionary<Condition, object> currentState)
    {
        Dictionary<Condition, object> res = new Dictionary<Condition, object>();
        foreach (KeyValuePair<Condition, object> kvp in preConditions)
        {
            if(!currentState.ContainsKey(kvp.Key) || !currentState[kvp.Key].Equals(kvp.Value))
            {
                res.Add(kvp.Key, kvp.Value);
            }
        }
        return res;
    }

    public virtual bool CheckPreConditions(Dictionary<Condition, object> state)
    {
        return GetPreConditionsToFulFill(state).Count == 0;
    }

    /// <summary>
    /// Takes in a state and applies the actions post conditions to it. Returns a new state
    /// </summary>
    /// <param name="newState">Updated world state</param>
    public virtual Dictionary<Condition, object> ApplyPostConditionsToState(Dictionary<Condition, object> curState)
    {
        Dictionary<Condition, object> newState = new Dictionary<Condition, object>(curState);
        foreach(KeyValuePair<Condition, object> kvp in postConditions)
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

    public abstract IEnumerator DoAction();
    protected abstract void AddPreConditions();
    protected abstract void AddPostConditions();
}
