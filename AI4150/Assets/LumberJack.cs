using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LumberJack : NPC
{
    private new void Awake()
    {
        base.Awake();
        goal1 = new Goal("Get Axe", new KeyValuePair<Condition, object>(Condition.hasSword, true));
        goal2 = new Goal("Get wood", new KeyValuePair<Condition, object>(Condition.hasWood, true));
    }

    public void INeedAxe()
    {
        Dictionary<Condition, object> gd = new Dictionary<Condition, object>();
        gd.Add(Condition.hasAxe, true);
        Queue<GOAPAction> plan = planner.CreatePlan(this, gd);
        if (plan != null)
        {
            StartCoroutine(DoPlan(plan, " Craft Axe"));
        }
        else
        {
            UI_Plan_Text.text = "Failed plan";
        }
    }

    public void INeedWood()
    {
        Dictionary<Condition, object> gd = new Dictionary<Condition, object>();
        gd.Add(Condition.hasWood, true);
        Queue<GOAPAction> plan = planner.CreatePlan(this, gd);
        if (plan != null)
        {
            StartCoroutine(DoPlan(plan, " Get Wood"));
        }
        else
        {
            UI_Plan_Text.text = "Failed plan";
        }
    }

    public override void SetUI()
    {
        moneyText.text = "Money: " + money;
        SetButton(goal1Button, goal1);
        SetButton(goal2Button, goal2);
        UI_Plan_Text.text = currentPlan;
        goal1Button.GetComponent<Button>().onClick.AddListener(() => INeedAxe());
        goal2Button.GetComponent<Button>().onClick.AddListener(() => INeedWood());
    }
}
