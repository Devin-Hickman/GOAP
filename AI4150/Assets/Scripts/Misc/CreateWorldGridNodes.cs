using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWorldGridNodes : MonoBehaviour
{
    //These items are set in the inspector
    public Transform startCords;
    public Transform endCords;

    // Start is called before the first frame update
    void Start()
    {
        for(int x = (int)startCords.position.x; x <= endCords.position.x; x++)
        {
            for(int y = (int)endCords.position.y; y <= endCords.position.y; y++)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
