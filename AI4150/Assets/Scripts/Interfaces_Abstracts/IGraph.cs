using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public delegate INode FindNextNode(INode start, INode end);

public interface IGraph
{
    List<INode> GetGraph();
    List<INode> BuildGraph();

    // the public search is just a public wrapper on private search that throws the heuristic at the private search
    void Search(INode start, INode end);
    //void Search(INode start, INode end, FindNextNode heuristic);
}
