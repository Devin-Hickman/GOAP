using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    [SerializeField]
    private PathfindingGraph pathFinder;

    private void Awake()
    {
        pathFinder = GameObject.Find("PathGraph").GetComponent<PathfindingGraph>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GeneratePathForNPC();
    }

    private void GeneratePathForNPC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse clicked @ " + pos.x + "," + pos.y);
            pathFinder.GetNearestNode(pos.x, pos.y);
        }
    }
}
