using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPNode : AbstractNode
{
    public GOAPNode parent;
    public GOAPAction action;
    public float costSoFar;
    public Dictionary<Condition, object> state;

    public GOAPNode(GOAPNode p, GOAPAction a, Dictionary<Condition,object> s, float c)
    {
        parent = p;
        action = a;
        state = s;
        costSoFar = c;
    }
}
