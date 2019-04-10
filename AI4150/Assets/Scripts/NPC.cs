using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private Dictionary<Condition, object> npcState = new Dictionary<Condition, object>();
    public HashSet<GOAPAction> AllActions { get; } = new HashSet<GOAPAction>();
    private HashSet<GOAPAction> currentUsableActions = new HashSet<GOAPAction>();
    GOAPPlanner planner = new GOAPPlanner();

    public float moveSpeed;
    public Vector2 velocity;
    public int money = 0;

    public float reachRadius;
    public float interactableSearchRadius;

    public Text moneyText;
    private string defaultMoneyText;
    // Start is called before the first frame update
    void Awake()
    {
        //Creates NPC State
        defaultMoneyText = moneyText.text;
        foreach(Condition c in Enum.GetValues(typeof(Condition))){
            switch (c)
            {
                case Condition.nearIron:
                    npcState.Add(c, float.MaxValue);
                    break;
                case Condition.nearShop:
                    npcState.Add(c, float.MaxValue);
                    break;
                default:
                    npcState.Add(c, false);
                    break;
            }
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
        FindInteractablesInMoveRadius();
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

    public void FindInteractablesInMoveRadius()
    {
        ContactFilter2D cf2d = new ContactFilter2D();
        cf2d.layerMask = LayerMask.GetMask("Items");
        cf2d.useLayerMask = true;

        CircleCollider2D reachedRadius = gameObject.AddComponent<CircleCollider2D>();
        reachedRadius.radius = interactableSearchRadius;
        reachedRadius.isTrigger = true;

        Collider2D[] interactableColliders = new Collider2D[100];
        int numInteractables = reachedRadius.OverlapCollider(cf2d, interactableColliders);

        if (numInteractables != 0)
        {
            for (int i = 0; i < numInteractables; i++)
            {
                float dist = Mathf.Abs(Vector3.Distance(transform.position, interactableColliders[i].transform.position));
                PredictMove.AddComponent(gameObject, dist, interactableColliders[i].transform.position, moveSpeed, interactableColliders[i].name, i);
                GetComponents<PredictMove>()[i].CreatePostConditions(npcState);
            }
        }
        //Destroy(reachedRadius);
    }

    public void AddGOAPAction(GOAPAction a)
    {
        AllActions.Add(a);
    }

    public void SetMoney(int m)
    {
        money = m;
        moneyText.text = defaultMoneyText + money;
    }
}
