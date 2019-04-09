using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPNode : AbstractNode
{
    public GOAPNode parent;
    public GOAPAction action;
    public float costSoFar;
    public Dictionary<Condition, bool> state;

    public GOAPNode(GOAPNode p, GOAPAction a, Dictionary<Condition,bool> s, float c)
    {
        parent = p;
        action = a;
        state = s;
        costSoFar = c;
    }
}
