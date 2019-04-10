using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSword : GOAPAction
{

    private new void Awake()
    {
        base.Awake();
        cost = 300f;
    }

    public override IEnumerator DoAction()
    {
        yield return CreateSword();
    }

    private IEnumerator CreateSword()
    {
        Debug.Log("Crafting my sword!");
        yield return new WaitForSeconds(2);
        GameObject sword = Instantiate(Resources.Load("Sword")) as GameObject;
        sword.GetComponent<SpriteRenderer>().sortingOrder = 1;
        sword.transform.position = this.transform.position;
    }

    protected override void AddPreConditions()
    {
        preConditions.Add(Condition.hasIron, true);
    }

    protected override void AddPostConditions()
    {
        postConditions.Add(Condition.hasSword, true);
    }
}
