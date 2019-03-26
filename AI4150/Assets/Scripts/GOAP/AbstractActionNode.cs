using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractActionNode : AbstractNode
{
    protected List<AbstractCondition> preConditions;
    protected List<AbstractCondition> postConditions;

    public List<AbstractCondition> GetPreConditionsToFulFill()
    {
        List<AbstractCondition> res = new List<AbstractCondition>();
        foreach(AbstractCondition a in preConditions)
        {
            if(!a.IsCompleted()) { res.Add(a); }
        }
        return res;
    }

    public bool CheckPreConditions()
    {
        foreach (AbstractCondition a in preConditions)
        {
            if (!a.IsCompleted()) { return false;}
        }
        return true;
    }
    
    public void FulFillPostConditions()
    {
        foreach(AbstractCondition a in postConditions)
        {
            a.Complete();
        }
    }

    public abstract void DoAction();
}
