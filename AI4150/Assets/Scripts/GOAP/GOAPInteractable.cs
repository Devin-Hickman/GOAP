using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPInteractable
{
    public abstract Dictionary<Condition, object> ApplyPostConditions(Dictionary<Condition, object> curState, float dist);
}
