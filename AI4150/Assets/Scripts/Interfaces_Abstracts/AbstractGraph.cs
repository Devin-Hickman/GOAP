using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGraph : MonoBehaviour, IGraph
{
    public abstract void BuildGraph();

    public List<INode> GetGraph()
    {
        throw new System.NotImplementedException();
    }

    public abstract void Search(AbstractNode start, AbstractNode end);

    //public abstract void Search(INode start, INode end, FindNextNode heuristic);
}
