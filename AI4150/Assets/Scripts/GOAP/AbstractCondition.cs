using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCondition 
{
    private bool completion = false;
    public bool IsCompleted()
    {
        return completion;
    }

    public void Complete()
    {
        completion = true;
    }
}
