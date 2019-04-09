using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField]
    private PathfindingGraph pathFinder;

    private void Awake()
    {
        pathFinder = GameObject.Find("PathGraph").GetComponent<PathfindingGraph>();
        rb2d = GetComponent<Rigidbody2D>();
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
            PathFindingNode start = pathFinder.GetNearestNode(pos.x, pos.y);

            // get nearest node of NPC
            Vector3 npcPos = rb2d.position;
            PathFindingNode end = pathFinder.GetNearestNode(npcPos.x, npcPos.y);

            pathFinder.Search(start, end);
        }
    }
}
