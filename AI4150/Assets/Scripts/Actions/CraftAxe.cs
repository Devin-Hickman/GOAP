using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftAxe : GOAPAction
{
    private new void Awake()
    {
        base.Awake();
        cost = 500;
    }

    public override IEnumerator DoAction(Dictionary<Condition, object> npcState)
    {
        base.DoAction(npcState);
        yield return CreateAxe();
    }

    private IEnumerator CreateAxe()
    {
        Debug.Log("Crafting my axe!");
        yield return new WaitForSeconds(2);
        GameObject axe = Instantiate(Resources.Load("Axe")) as GameObject;
        axe.GetComponent<SpriteRenderer>().sortingOrder = 1;
        axe.transform.position = this.transform.position;
    }

    protected override void AddPreConditions()
    {
        preConditions.Add(Condition.hasIron, true);
        preConditions.Add(Condition.hasWood, true);
    }

    protected override void AddPostConditions()
    {
        postConditions.Add(Condition.hasAxe, true);
    }
}
