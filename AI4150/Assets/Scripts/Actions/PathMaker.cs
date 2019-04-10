using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    Rigidbody2D rb2d;
    private int currentPathIndex = 0;
    List<PathFindingNode> path;
    [SerializeField]
    private PathfindingGraph pathFinder;
    public float speed = 1;

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
        // get nearest node of NPC
        Vector3 npcPos = rb2d.position;
        PathFindingNode start = pathFinder.GetNearestNode(npcPos.x, npcPos.y);
        GeneratePathForNPC(start);
        MoveNPCWithPath(start);
    }

    private void GeneratePathForNPC(PathFindingNode start)
    {
        if (Input.GetMouseButtonDown(0))
        {
            path = null;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Mouse clicked @ " + pos.x + "," + pos.y);
            PathFindingNode end = pathFinder.GetNearestNode(pos.x, pos.y);

            pathFinder.Search(start, end);
            currentPathIndex = 0;
        }
    }
    
    private void MoveNPCWithPath(PathFindingNode currentNode)
    {
        // get the path in pathFindingGraph
        // store what index we think we are on. 
        // check if start has the same name as the index we are on if it is the same then we increment the index
        // move towards path[currentIndex]
        path = pathFinder.PathSoFar;
        if(path != null && path.Count > 0)
        {
            if(path[currentPathIndex].GetName.Equals(currentNode.GetName) && currentPathIndex != path.Count - 1)
            {
                currentPathIndex++;
            } else if(!path[currentPathIndex].GetName.Equals(currentNode.GetName))
            {
                // move towards path[currentPathIndex]
                MoveTowardsNode(path[currentPathIndex]);
            }
        }
    }

    private void MoveTowardsNode(PathFindingNode node) 
    {
        Vector3 npcPos = rb2d.position;
        float xVelocity = 0;
        float yVelocity = 0;
        if (node.GetX > npcPos.x)
        {
            xVelocity = speed;
        }
        else if (node.GetX < npcPos.x)
        {
            xVelocity = -speed;
        }
        if (node.GetY > npcPos.y)
        {
            yVelocity = speed;
        }
        else if (node.GetY < npcPos.y)
        {
            yVelocity = -speed;
        }
        rb2d.velocity = new Vector2(xVelocity, yVelocity);
    }
}
