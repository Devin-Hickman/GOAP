using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Dictionary<Condition, object> npcState = new Dictionary<Condition, object>();
    public HashSet<GOAPAction> AllActions { get; }
    private HashSet<GOAPAction> currentUsableActions = new HashSet<GOAPAction>();
    // Start is called before the first frame update
    void Awake()
    {
        //Creates NPC State
        foreach(Condition c in Enum.GetValues(typeof(Condition))){
            npcState.Add(c,false);
        }
    }

    private void Start()
    {
        //Sets up all actions NPC can take & actions NPC can use right now
        foreach (GOAPAction action in GetComponents<GOAPAction>())
        {
            AllActions.Add(action);
            if (action.CheckPreConditions(npcState))
            {
                currentUsableActions.Add(action);
            }
        }
    }

    public Dictionary<Condition, object> GetNPCState()
    {
        return npcState;
    }
}
