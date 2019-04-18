using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGizmos : MonoBehaviour
{
    public bool walkable;
    private float size;

    public void SetValues(float x_, float y_, bool w, string s, float size)
    {
        this.transform.position = new Vector3(x_, y_, 1);
        walkable = w;
        this.name = s;
        this.size = size;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, size);
        
    }
}
