using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySword : GOAPAction
{
    int swordPrice = 0;

    private new void Awake()
    {
        base.Awake();
        swordPrice = GameObject.Find("Shop").GetComponent<ShopInventory>().swordPrice;
        cost = 10;
    }

    public override void DoAction()
    {
        Debug.Log("Bought a sword!");
        GameObject sword = Instantiate(Resources.Load("Sword")) as GameObject;
        sword.GetComponent<SpriteRenderer>().sortingOrder = 1;
        sword.transform.position = this.transform.position;
        GetComponent<NPC>().SetMoney(GetComponent<NPC>().money - swordPrice);
    }

    protected override void AddPostConditions()
    {
        postConditions.Add(Condition.hasSword, true);
    }

    protected override void AddPreConditions()
    {
        preConditions.Add(Condition.canAfford, true);
        preConditions.Add(Condition.nearShop, 5.0f);
    }

    public override bool CheckPreConditions(Dictionary<Condition, object> state)
    {
        return GetComponent<NPC>().money >= swordPrice 
            && state.ContainsKey(Condition.nearShop) 
            && (float)state[Condition.nearShop] <= (float)preConditions[Condition.nearShop];
    }
}
