using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iGraph
{
    private List<iNode> graph;

    public List<iNode> buildGraph();

    private delegate iNode findNextNode(iNode start, iNode end);

    // the public search is just a public wrapper on private search that throws the heuristic at the private search
    public void search(iNode start, iNode end);
    private void privateSearch(iNode start, iNode end, findNextNode heuristic);
}
