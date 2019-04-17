using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    Rigidbody2D rb2d;
    private int currentPathIndex = 0;
    List<AbstractNode> path;
    [SerializeField]
    private PathfindingGraph pathFinder;
    private float speed;
    private NPC n;
    private bool rightClick = false;

    private void Awake()
    {
        pathFinder = GameObject.Find("PathGraph").GetComponent<PathfindingGraph>();
        rb2d = GetComponent<Rigidbody2D>();
        speed = GetComponent<NPC>().moveSpeed;
        n = GetComponent<NPC>();
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

    public IEnumerator MoveTowards(Vector2 pos)
    {
        PathFindingNode start = pathFinder.GetNearestNode(rb2d.position.x, rb2d.position.y);
        PathFindingNode end = pathFinder.GetNearestNode(pos.x, pos.y);

        pathFinder.Search(start, end);
        currentPathIndex = 0;

        yield return MoveNPCWithPath(start,end);
    }

    private void GeneratePathForNPC()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightClick = true;
            Vector3 npcPos = rb2d.position;
            PathFindingNode start = pathFinder.GetNearestNode(npcPos.x, npcPos.y);
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathFindingNode end = pathFinder.GetNearestNode(pos.x, pos.y);
            path = null;
            rightClick = false;
            pathFinder.Search(start, end);
            currentPathIndex = 0;

            StopCoroutine(MoveNPCWithPath(start,end));
            StartCoroutine(MoveNPCWithPath(start,end));

        }
    }
    
    private IEnumerator MoveNPCWithPath(AbstractNode currentNode, AbstractNode end)
    {
        path = pathFinder.PathSoFar;
        if (path == null || path.Count == 0) yield break;
        // get the path in pathFindingGraph
        // store what index we think we are on. 
        // check if start has the same name as the index we are on if it is the same then we increment the index
        // move towards path[currentIndex]
        while (currentNode != end && currentPathIndex < path.Count)
        {
            if(path == null || rightClick)
            {
                yield break;
            }
            currentNode = path[currentPathIndex];
            yield return Move(new Vector2(currentNode.GetX, currentNode.GetY));
            currentPathIndex++;

                /*if (path[currentPathIndex].GetName.Equals(currentNode.GetName) && currentPathIndex != path.Count - 1)
                {
                    currentPathIndex++;
                }
                else if (!path[currentPathIndex].GetName.Equals(currentNode.GetName))
                {
                    // move towards path[currentPathIndex]

                }*/            
        }
        if (currentNode == end)
        {
            yield return Move(new Vector2(end.GetX, end.GetY));
        }
    }

    protected IEnumerator Move(Vector2 destination)
    {
        Debug.Log("Moving");
        float dist = Vector2.Distance(n.transform.position, destination);
        while (dist > 0.1)
        {
            Vector2 dir = destination - (Vector2)n.transform.position;
            rb2d.velocity = (dir.normalized * speed);
            dist = Vector2.Distance(n.transform.position, destination);
            yield return null;
        }
        rb2d.velocity = (Vector2.zero);
    }
}
