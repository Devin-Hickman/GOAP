using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractActionNode : MonoBehaviour
{
    protected Dictionary<Condition, object> preConditions;
    protected Dictionary<Condition, object> currentConditions;
    protected Dictionary<Condition, object> postConditions;

    public List<KeyValuePair<Condition, object>> GetPreConditionsToFulFill()
    {
        List<KeyValuePair<Condition, object>> res = new List<KeyValuePair<Condition, object>>();
        foreach (KeyValuePair<Condition, object> kvp in preConditions)
        {
            if (kvp.Value != currentConditions[kvp.Key])
            {
                res.Add(kvp);
            }
        }
        return res;
    }

    public bool CheckPreConditions()
    {
        List<KeyValuePair<Condition, object>> tmp = GetPreConditionsToFulFill();
        return tmp.Count == 0;
    }

    public void FulFillPostConditions()
    {
        foreach(KeyValuePair<Condition, object> kvp in postConditions)
        {
            if (currentConditions.ContainsKey(kvp.Key))
            {
                currentConditions[kvp.Key] = kvp.Value;
            }
            else
            {
                currentConditions.Add(kvp.Key, kvp.Value);
            }
        }
    }

    public abstract void DoAction();
}
