using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGraph : iGraph
{
    private Queue<PathFindingNode> pathSoFar = new Queue<PathFindingNode>();
    List<PathFindingNode> topLevelPath = new List<PathFindingNode>();
    List<PathFindingNode> midLevelPath = new List<PathFindingNode>();

    public List<iNode> buildGraph()
    {

    }

    // gets the neighbors of start and returns the one that is closest to end by euclidian distance
    private PathfindingNode returnNodeWithShortestDistToTarget(PathfindingNode start, PathfindingNode end)
    {
    }

    //public void search(PathfindingNode start, PathfindingNode end)
    //{
    //    findNextNode heuristic = returnNodeWithShortestDistToTarget;
    //    privateSearch(start, end, heuristic);

    //}

    /**
    * Search
    * The nodes it is passed must be bottom level nodes if they are regional or parent nodes then this won't work... 
    * These bottom level nodes most
    * 
    */
    // this assumes there is always a path to a given location
    public void search(PathfindingNode start, PathfindingNode end)
    {
        if(start.parentRegion.parentRegion == null || end.parentRegion.parentRegion == null)
        {
            throw new Exception("start and end nodes must both have two layers of parents above them.");
        }

        if(!start.parentRegion.parentRegion.equals(end.parentRegion.parentRegion))
        {
            // generate the toplevel path then 
            topLevelPath = aStar(start.parentRegion.parentRegion, end.parentRegion.parentRegion);
            for (int i = 1; i < topLevelPath.size(); i++)
            {
                // initi
                PathfindingNode midLevelStart = start.parentRegion;
                if (midLevelPath.Count > 0)
                {
                    midLevelStart = midLevelPath[midLevelPath.size() - 1];
                }
                updateMidLevelPath(start, midLevelStart, topLevelPath[i]);
            }
        } else if (!start.parentRegion.equals(end.parentRegion))
        {
            // do the mid level Path
            updateMidLevelPath(start, start.parentRegion, end.parentRegion);
        } else
        {
            // do straight A* between the nodes and return that path.
            updateBottomLevelPath(start, end);
        }
    }

    List<PathfindingNode> updateBottomLevelPath(PathfindingNode start, PathfindingNode end)
    {
        List<PathFindingNode> bottomLevelPath = aStar(start, end);
        for (int i = 0; i < bottomLevelPath.size(); i++)
        {
            pathSoFar.Enqueue(bottomLevelPath[i]);
        }
        return bottomLevelPath;
    }

    List<PathFindingNode> updateMidLevelPath(PathfindingNode start, PathfindingNode firstMidLevel, PathfindingNode end) {
        midLevelPath = aStar(firstMidLevel, end);
        List<PathFindingNode> bottomLevelPath = new List<PathFindingNode>();
        PathFindingNode bottomStart = start;
        for (int i = 1; i < midLevelPath.size(); i++)
        {
            if (bottomLevelPath.size() > 0)
            {
                bottomStart = bottomLevelPath[bottomLevelPath.size() - 1];
            }
            bottomLevelPath = updateBottomLevelPath(bottomStart, midLevelPath[i]);
        }
    }

    // do A star 
    // kick out if the parent changes or if you reach end
    private List<PathFindingNode> aStar(PathFindingNode start, PathFindingNode end)
    {
        // do A* from start to end but kick out the path if you enter a new region or sub region
        
    }
}
