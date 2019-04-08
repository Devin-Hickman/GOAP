using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGizmos : MonoBehaviour
{
    public bool walkable;


    public void SetValues(int x_, int y_, bool w, string s)
    {
        this.transform.position = new Vector3(x_, y_, 1);
        walkable = w;
        this.name = s;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, .15f);
        
    }
}
