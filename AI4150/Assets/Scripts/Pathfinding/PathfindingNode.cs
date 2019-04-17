using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFindingNode : AbstractNode
{
    public bool IsWalkable { get; set; }
    public List<INode> Neighbors { get { return neighbors; } set { neighbors = value; } }
    public void AddNeighbor(PathFindingNode n) { neighbors.Add(n); }

    public PathFindingNode(Vector2 pos_, bool walkable)
    {
        Pos = pos_;
        IsWalkable = walkable;
        neighbors = new List<INode>();
        
    }

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
            return (GetX == p.GetX) && (GetY == p.GetY);
        }
    }

    public override int Heuristic(AbstractNode toBeCompared, AbstractNode goalNode)
    {
        double thisDistance = Math.Sqrt(Math.Pow(this.GetX - goalNode.GetX, 2) + Math.Pow(this.GetY - goalNode.GetY, 2));
        double otherDistance = Math.Sqrt(Math.Pow(toBeCompared.GetX - goalNode.GetX, 2) + Math.Pow(toBeCompared.GetY - goalNode.GetY, 2));
        if (thisDistance < otherDistance) return -1;
        else if (thisDistance > otherDistance) return 1;
        else return 0;
    }
}

