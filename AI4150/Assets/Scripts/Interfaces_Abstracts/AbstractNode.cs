﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbstractNode : INode
{
    public String GetName { get { return string.Format("{0:N1}", this.GetX) + "," + string.Format("{0:N1}", this.GetY);  } }
    public Vector2 Pos { get; set; }
    public float GetX { get { return Pos.x; } }
    public float GetY { get { return Pos.y; } }
    protected List<INode> neighbors;
    public float CostSoFar { get; set; }
    public AbstractNode Parent { get; set; }
    public ParentRegionNode ParentRegion { get; set; }
    public List<INode> GetNeighbors()
    {
        return neighbors;
    }

    public abstract int Heuristic(AbstractNode toBeCompared, AbstractNode goalNode);
}
