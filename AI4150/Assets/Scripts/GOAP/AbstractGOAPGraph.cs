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
        GOAPAction endAction = (GOAPAction)end;

        throw new System.NotImplementedException();
    }


}
