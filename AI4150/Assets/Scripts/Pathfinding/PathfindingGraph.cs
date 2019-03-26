using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGraph : AbstractGraph
{
    private Queue<PathFindingNode> pathSoFar = new Queue<PathFindingNode>();
    List<PathFindingNode> topLevelPath = new List<PathFindingNode>();
    List<PathFindingNode> midLevelPath = new List<PathFindingNode>();

    public override List<INode> BuildGraph()
    {
        List<INode> res = new List<INode>();

        return res;
    }

    // gets the neighbors of start and returns the one that is closest to end by euclidian distance
    private PathFindingNode returnNodeWithShortestDistToTarget(PathFindingNode start, PathFindingNode end)
    {
        PathFindingNode res = null;

        return res;
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
    public void search(PathFindingNode start, PathFindingNode end)
    {
        if(start.GetParentRegion().GetParentRegion() == null || end.GetParentRegion().GetParentRegion() == null)
        {
            //Is this true? What about searching on the top-most level?
            throw new System.Exception("start and end nodes must both have two layers of parents above them.");
        }

        if(!start.GetParentRegion().GetParentRegion().Equals(end.GetParentRegion().GetParentRegion()))
        {
            // generate the toplevel path then 
            topLevelPath = aStar(start.GetParentRegion().GetParentRegion(), end.GetParentRegion().GetParentRegion());
            for (int i = 1; i < topLevelPath.Count; i++)
            {
                // initi
                PathFindingNode midLevelStart = start.GetParentRegion();
                if (midLevelPath.Count > 0)
                {
                    midLevelStart = midLevelPath[midLevelPath.Count - 1];
                }
                updateMidLevelPath(start, midLevelStart, topLevelPath[i]);
            }
        } else if (!start.GetParentRegion().Equals(end.GetParentRegion()))
        {
            // do the mid level Path
            updateMidLevelPath(start, start.GetParentRegion(), end.GetParentRegion());
        } else
        {
            // do straight A* between the nodes and return that path.
            updateBottomLevelPath(start, end);
        }
    }

    List<PathFindingNode> updateBottomLevelPath(PathFindingNode start, PathFindingNode end)
    {
        List<PathFindingNode> bottomLevelPath = aStar(start, end);
        for (int i = 0; i < bottomLevelPath.Count; i++)
        {
            pathSoFar.Enqueue(bottomLevelPath[i]);
        }
        return bottomLevelPath;
    }

    List<PathFindingNode> updateMidLevelPath(PathFindingNode start, PathFindingNode firstMidLevel, PathFindingNode end) {
        List<PathFindingNode> result = new List<PathFindingNode>();
        midLevelPath = aStar(firstMidLevel, end);
        List<PathFindingNode> bottomLevelPath = new List<PathFindingNode>();
        PathFindingNode bottomStart = start;
        for (int i = 1; i < midLevelPath.Count; i++)
        {
            if (bottomLevelPath.Count > 0)
            {
                bottomStart = bottomLevelPath[bottomLevelPath.Count - 1];
            }
            bottomLevelPath = updateBottomLevelPath(bottomStart, midLevelPath[i]);
        }
        return result;
    }

    // do A star 
    // kick out if the parent changes or if you reach end
    private List<PathFindingNode> aStar(PathFindingNode start, PathFindingNode end)
    {
        List<PathFindingNode> path = new List<PathFindingNode>();
        // do A* from start to end but kick out the path if you enter a new region or sub region

        return path;
        
    }

    public override void Search(INode start, INode end)
    {
        throw new System.NotImplementedException();
    }

    public override void Search(INode start, INode end, FindNextNode heuristic)
    {
        throw new System.NotImplementedException();
    }
}
