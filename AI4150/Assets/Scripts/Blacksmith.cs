using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blacksmith : NPC
{
    private new void Awake()
    {
        base.Awake();
        goal1 = new Goal("Create Sword", new KeyValuePair<Condition, object>(Condition.hasSword, true));
    }

    public void INeedSword()
    {
        Debug.Log("Getting sword");
        Dictionary<Condition, object> gd = new Dictionary<Condition, object>();
        gd.Add(Condition.hasSword, true);
        Queue<GOAPAction> plan = planner.CreatePlan(this, gd);
        if (plan != null)
        {
            StartCoroutine(DoPlan(plan, " Craft sword"));
        }
    }

    public override void SetUI()
    {
        moneyText.text = "Money: " + money;
        SetButton(goal1Button, goal1);
        UI_Plan_Text.text = currentPlan;
        goal1Button.GetComponent<Button>().onClick.AddListener(() => INeedSword());
    }

}
