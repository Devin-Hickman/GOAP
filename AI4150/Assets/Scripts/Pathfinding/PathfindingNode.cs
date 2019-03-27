using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode : AbstractNode
{
    public Vector2 Pos { get; set; }
    public float GetX { get { return Pos.x; } }
    public float GetY { get { return Pos.y; } }
    public bool IsWalkable { get; set; }
    public List<INode> Neighbors { get { return neighbors; } set { neighbors = value; } }
    public void AddNeighbor(PathFindingNode n) { neighbors.Add(n); }

    public PathFindingNode(Vector2 pos_, bool walkable)
    {
        Pos = pos_;
        IsWalkable = walkable;
        neighbors = new List<INode>();
        
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
            return (GetX == p.GetX) && (GetY == p.GetY);
        }
    }
}

