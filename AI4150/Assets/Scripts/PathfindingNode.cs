using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : iNode
{
    private double x
    {
        get;
        set;
    }
    private double y
    {
        get;
        set;
    }

    private PathfindingNode parentRegion;

    public List<PathfindingNode> getNeighbors()
    {
        return this.neighbors;
    }

    public override bool Equals(Object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            PathfindingNode p = (PathfindingNode)obj;
            return (x == p.x) && (y == p.y);
        }
    }
}

