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
        neighbors = new List<INode>();
    }

    public void AddToChildNodes(AbstractNode childNode)
    {
        ChildNodes.Add(childNode);
    }

    /**
     * The child node that is on the border of another node on this level
     * The sprite is the sprite of the bordering node on this level
     * The neighbor is the node of the neighbor itself
     * 
     * */
    public void AddToBorderNodes(AbstractNode childNode, string sprite, AbstractNode neighbor)
    {
        if(!BorderNodes.ContainsKey(sprite)) {
            BorderNodes[sprite] = new List<AbstractNode>();
        }
        BorderNodes[sprite].Add(childNode);

        if(!neighbors.Contains(neighbor))
        {
            neighbors.Add(neighbor);
        }
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

    // This looks at all of the border nodes, finds the closest one to the position and then returns it
    public AbstractNode GetClosestBorderNode(Vector2 pos, string targetRegion)
    {
        if(!BorderNodes.ContainsKey(targetRegion))
        {
            throw new Exception("Target region is not a neighbor");
        }
        double shortestDist = Double.MaxValue;
        AbstractNode bestNode = null;
        foreach(AbstractNode node in BorderNodes[targetRegion])
        {
            double temp = Math.Sqrt(Math.Pow(pos.x - node.GetX, 2) + Math.Pow(pos.y - node.GetY, 2));
            if(temp < shortestDist)
            {
                bestNode = node;
            }
        }
        return bestNode;
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
