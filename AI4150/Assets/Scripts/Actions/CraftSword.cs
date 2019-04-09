using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSword : GOAPAction
{

    private new void Awake()
    {
        base.Awake();
        //preConditions.Add(Condition.hasIron, true);
        postConditions.Add(Condition.hasSword, true);
    }

    public CraftSword()
    {
        postConditions.Add(Condition.hasSword, true);
    }

    public override void DoAction()
    {
        StartCoroutine(CreateSword());
    }

    private IEnumerator CreateSword()
    {
        Debug.Log("Crafting my sword!");
        yield return new WaitForSeconds(10);
        GameObject sword = Instantiate(Resources.Load("Sword")) as GameObject;
        sword.transform.position = this.transform.position;
    }
}
