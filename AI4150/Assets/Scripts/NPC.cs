using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class NPC : MonoBehaviour
{

    protected class Goal
    {
        public string goalName;
        public KeyValuePair<Condition, object> postGoal;

        public Goal(string s, KeyValuePair<Condition, object> goal)
        {
            goalName = s;
            postGoal = goal;
        }
    }

    private Dictionary<Condition, object> npcState = new Dictionary<Condition, object>();
    public HashSet<GOAPAction> AllActions { get; } = new HashSet<GOAPAction>();
    private HashSet<GOAPAction> currentUsableActions = new HashSet<GOAPAction>();
    protected GOAPPlanner planner;

    public float moveSpeed;
    public Vector2 velocity;
    public int money = 0;

    public float reachRadius;
    public float interactableSearchRadius;

    static GameObject npcUI;

    public Text moneyText;
    private string defaultMoneyText;

    public Text UI_Plan_Text;
    protected string currentPlan;
    protected Goal goal1;
    public Button goal1Button;
    protected Goal goal2;
    public Button goal2Button;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //Creates NPC State
        planner = GetComponent<GOAPPlanner>();
        npcUI = GameObject.Find("NPC UI");
        currentPlan = "No plan";
        foreach(Condition c in Enum.GetValues(typeof(Condition))){
            switch (c)
            {
                case Condition.nearIron:
                    npcState.Add(c, float.MaxValue);
                    break;
                case Condition.nearShop:
                    npcState.Add(c, float.MaxValue);
                    break;
                case Condition.nearWood:
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

    protected IEnumerator DoPlan(Queue<GOAPAction> plan, string goal)
    {
        currentPlan = "Current plan" + goal;
        UI_Plan_Text.text = currentPlan;
        while (plan.Count > 0)
        {
            yield return plan.Dequeue().DoAction(npcState);
        }
        currentPlan = "No plan";
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
        Destroy(reachedRadius);
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

    private void OnMouseEnter()
    {
        Tooltip.DisplayTooltip(this.name);
    }

    private void OnMouseExit()
    {
        Tooltip.HideTooltip();
    }


    public virtual void SetUI()
    {

    }

    protected void SetButton(Button g, Goal goal)
    {
        if(g != null && goal != null)
        {
            g.gameObject.SetActive(true);
            g.GetComponentInChildren<Text>().text = goal.goalName;
        }
    }

    public virtual void ClearUI()
    {
        if(goal1Button != null)
        {
            goal1Button.gameObject.SetActive(false);
            goal1Button.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        if(goal2Button != null)
        {
            goal2Button.gameObject.SetActive(false);
            goal2Button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        UI_Plan_Text.text = "";
        moneyText.text = "";    
    }
}
