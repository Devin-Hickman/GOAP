using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlanner : MonoBehaviour
{
    private PathfindingGraph pathFinder;
    private void Awake()
    {
        GameObject theGrid = GameObject.Find("PathFindingGraph");
        pathFinder = theGrid.GetComponent<PathfindingGraph>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GeneratePathForNPC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newPosition = hit.point;
                transform.position = newPosition;
                //pathFinder.GetNearestNode(transform.TransformPoint.);
            }
        }
    }
}
