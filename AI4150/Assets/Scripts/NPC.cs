using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private Dictionary<Condition, object> npcState = new Dictionary<Condition, object>();
    public HashSet<GOAPAction> AllActions { get; } = new HashSet<GOAPAction>();
    private HashSet<GOAPAction> currentUsableActions = new HashSet<GOAPAction>();
    GOAPPlanner planner = new GOAPPlanner();

    public Vector2 velocity;

    public float ReachRadius { get; set; }
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

    public void INeedSword()
    {
        Debug.Log("I need a sword!");
        Dictionary<Condition, object> gd = new Dictionary<Condition, object>();
        gd.Add(Condition.hasSword, true);
        Queue<GOAPAction> plan = planner.CreatePlan(this, gd);
        if(plan != null)
        {
            StartCoroutine(DoPlan(plan));
        }
    }

    private IEnumerator DoPlan(Queue<GOAPAction> plan)
    {
        Debug.Log("Time to do this thing!");
        while(plan.Count > 0)
        {
            plan.Dequeue().DoAction();
            yield return null;
        }
    }

    public void MoveTo()
    {

    }
}
