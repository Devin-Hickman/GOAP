using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public delegate INode FindNextNode(INode start, INode end);

public interface IGraph
{
    List<INode> GetGraph();
    void BuildGraph();

    // the public search is just a public wrapper on private search that throws the heuristic at the private search
    void Search(AbstractNode start, AbstractNode end);
    //void Search(INode start, INode end, FindNextNode heuristic);
}
