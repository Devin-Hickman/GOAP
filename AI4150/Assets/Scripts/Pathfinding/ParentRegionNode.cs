using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParentRegionNode : AbstractNode
{
    // This is the string for the tile image that this node has
    public string TileSprite { get; }

    // child nodes that are on the border of a neighbor
    // The key is the tileSprite string and the node is thie child node
    private Dictionary<string, List<AbstractNode>> BorderNodes;
    // These are all of the nodes inside of this node one layer below this one in the heirarchy
    // this field can be cleared out after the average of them is calculated to compute the 
    // x and y position of this node
    private List<AbstractNode> ChildNodes;

    public ParentRegionNode(string tileSprite)
    {
        TileSprite = tileSprite;
        BorderNodes = new Dictionary<string, List<AbstractNode>>();
        ChildNodes = new List<AbstractNode>();
    }

    public void AddToChildNodes(AbstractNode childNode)
    {
        ChildNodes.Add(childNode);
    }

    public void AddToBorderNodes(AbstractNode childNode, string sprite)
    {
        if(!BorderNodes.ContainsKey(sprite)) {
            BorderNodes[sprite] = new List<AbstractNode>();
        }
        BorderNodes[sprite].Add(childNode);
    }

    /**
     * This calculates the position by averaging all of the children nodes.
     * Must only be called after all children nodes are added.
     * 
     **/
    public void CalculatePos()
    {
        float xSum = 0;
        float ySum = 0;
        foreach(AbstractNode child in ChildNodes)
        {
            xSum += child.GetX;
            ySum += child.GetY;
        }
        this.Pos = new Vector2(xSum / ChildNodes.Count, ySum / ChildNodes.Count);
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
