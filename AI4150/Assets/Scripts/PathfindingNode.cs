using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode : AbstractNode
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

    private PathFindingNode parentRegion;
    public PathFindingNode GetParentRegion() { return parentRegion; }

    public bool Equals(PathFindingNode obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            PathFindingNode p = obj;
            return (x == p.x) && (y == p.y);
        }
    }
}

