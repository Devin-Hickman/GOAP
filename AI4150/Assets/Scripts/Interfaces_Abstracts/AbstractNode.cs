using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractNode : INode
{
    protected List<INode> neighbors;

    public List<INode> GetNeighbors()
    {
        return neighbors;
    }
}
