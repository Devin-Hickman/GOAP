using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractGOAPGraph : AbstractGraph
{
    public override List<INode> BuildGraph()
    {
        throw new System.NotImplementedException();
    }

    public override void Search(INode start, INode end)
    {
        AbstractActionNode endAction = (AbstractActionNode)end;
        foreach(AbstractCondition a in endAction.GetPreConditionsToFulFill())
        {
            //Find actions that can fulfill the required preconditions
        }

        throw new System.NotImplementedException();
    }

    public override void Search(INode start, INode end, FindNextNode heuristic)
    {
        throw new System.NotImplementedException();
    }
}
