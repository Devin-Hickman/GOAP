using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class PathfindingGraph : MonoBehaviour, IGraph
{
    private Queue<PathFindingNode> pathSoFar = new Queue<PathFindingNode>();
    private Dictionary<string, PathFindingNode> bottomLevelDict = new Dictionary<string, PathFindingNode>();

    List<PathFindingNode> topLevelPath = new List<PathFindingNode>();
    List<PathFindingNode> midLevelPath = new List<PathFindingNode>();

    //List of grids. Index corresponds to level in hierarchy with 0 being lowest
    PathFindingNode[][,] hierarchicalGrids;

    //These values are set in Unity's Inspector
    public Transform gridStartPos;
    public Transform gridEndPos;
    public int gridCellSize; //Should be 1
    public List<Tilemap> tileMapRegions;  //Expects tileMaps in order of hierarchy with 0 being the lowest
    public Tilemap obstacleMap;

    PathVisualizer pathVisaulDebugger;
    private void Awake()
    {
        hierarchicalGrids = new PathFindingNode[tileMapRegions.Count][,];
        pathVisaulDebugger = GetComponent<PathVisualizer>();
    }

    void Start()
    {
        BuildGraph();
    }

    /**
     * Builds the first level of the graph using the tiles and nodes passed to it
     * 
     */
    private PathFindingNode[,] CreateLowLevelGrid()
    {
        int numCols = (int)Mathf.Abs((gridEndPos.position.x - gridStartPos.position.x) / gridCellSize);
        int numRows = (int)Mathf.Abs((gridEndPos.position.y - gridStartPos.position.y) / gridCellSize);
        return new PathFindingNode[numCols, numRows];
    }

    /**
     * passed in an x and y float and that is coerced to a position on the graph
     */ 
    public PathFindingNode GetNearestNode(float x, float y)
    {
        string key = string.Format("{0:N0}", Mathf.Round(x)) + "," + string.Format("{0:N0}", Mathf.Round(y));
        try
        {
            return this.bottomLevelDict[key];
        } catch
        {
            return null;
        }
    }

    /**
     * Builds the varying layers of the graph and updates the heirarchicalGrids object accordingly
     * 
     */
    public List<INode> BuildGraph()
    {
        List<INode> res = new List<INode>();
        BuildPathfindingNodes(0);
        // to build next level of nodes do clustering on the first level of nodes based off of their distance from an estimated center
        // instantiate clusters estimates with 5 times less nodes than the previous level
        // do the same thing again on that layer to generate the top level.
        //int layer = 1;
        //while(hierarchicalGrids[layer - 1].Length * hierarchicalGrids[layer - 1].Length > 100)
        //{

        //}
        //BuildPathfindingNodes(1);
        return res;
    }

    /**
     * Builds the lowest level of the graph using the tiles and updates the hierarchicalGrids object with that lowest level
     */
    private void BuildPathfindingNodes(int currentLevel)
    {
        int xIndex = -1;
        int yIndex = -1;

        hierarchicalGrids[currentLevel] = CreateLowLevelGrid();
        int nodesMade = 0;

        for (int x = (int)gridStartPos.position.x; x < gridEndPos.position.x; x += gridCellSize)
        {
            xIndex++;
            yIndex = -1;
            for (int y = (int)gridStartPos.position.y; y < gridEndPos.position.y; y += gridCellSize)
            {
                PathFindingNode tmp = null;
                TileBase tileBase = tileMapRegions[currentLevel].GetTile(new Vector3Int(x, y, 0));
                if (tileBase != null) // Tile exists
                {
                    nodesMade++;
                    TileBase obstacleBase = obstacleMap.GetTile(new Vector3Int(x, y, 0));
                    bool walkable = (obstacleBase == null) ? true : false;
                    tmp = new PathFindingNode(new Vector2(x, y), walkable);
                    CreateDebugNode(x, y, walkable, "L" + currentLevel + " N" +nodesMade);
                }
                else
                {
                    TileBase obstacleBase = obstacleMap.GetTile(new Vector3Int(x, y, 0));
                    if(obstacleBase != null)
                    {
                        nodesMade++;
                        tmp = new PathFindingNode(new Vector2(x, y), false);
                        CreateDebugNode(x, y, false, "L" + currentLevel + " N" + nodesMade);
                    }
                }
                yIndex++;
                if(tmp != null)
                {
                    bottomLevelDict.Add(tmp.GetName, tmp);
                }
                hierarchicalGrids[currentLevel][xIndex, yIndex] = tmp;
            }
        }
        AssignNodeNeighbors();
    }

    /**
    * Creates the nodes that are visible to the developer
    * 
    */
    private void CreateDebugNode(int x, int y, bool walkable, string name)
    {
        GameObject g = new GameObject();
        g.AddComponent<NodeGizmos>();
        g.GetComponent<NodeGizmos>().SetValues(x, y, walkable, name);
    }

    /**
    * Adds the neighboring nodes as neighbors to each node if they are walkable.
    * 
    */
    private void AssignNodeNeighbors()
    {
        foreach(PathFindingNode[,] grid in hierarchicalGrids)
        {
            if(grid != null)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] != null)
                        {
                            PathFindingNode curNode = grid[x, y];

                            int xTmp = x - 1;
                            if (xTmp > 0 && grid[xTmp, y] != null && grid[xTmp, y].IsWalkable) { curNode.AddNeighbor(grid[xTmp, y]); }

                            xTmp = x + 1;
                            if (xTmp < grid.GetLength(0) && grid[xTmp, y] != null && grid[xTmp, y].IsWalkable) { curNode.AddNeighbor(grid[xTmp, y]); }

                            int yTmp = y - 1;
                            if (yTmp > 0 && grid[x, yTmp] != null && grid[x, yTmp].IsWalkable) { curNode.AddNeighbor(grid[x, yTmp]); }

                            yTmp = y + 1;
                            if (yTmp < grid.GetLength(1) && grid[x, yTmp] != null && grid[x, yTmp].IsWalkable) { curNode.AddNeighbor(grid[x, yTmp]); }

                        }
                    }
                }
            }

        }
    }

    // gets the neighbors of start and returns the one that is closest to end by euclidian distance
    private PathFindingNode ReturnNodeWithShortestDistToTarget(PathFindingNode start, PathFindingNode end)
    {
        double shortestDistance = double.MaxValue;
        PathFindingNode closest = null;
        foreach (PathFindingNode neighbor in start.GetNeighbors())
        {
            if(neighbor != null)
            {
                double temp = Math.Sqrt(Math.Pow(neighbor.GetX - end.GetX, 2) + Math.Pow(neighbor.GetY - end.GetY, 2));
                if (temp < shortestDistance)
                {
                    shortestDistance = temp;
                }
            }
        }

        return closest;
    }


    //List<PathFindingNode> UpdateBottomLevelPath(PathFindingNode start, PathFindingNode end)
    //{
    //    List<PathFindingNode> bottomLevelPath = AStar(start, end);
    //    pathVisaulDebugger.DrawLines(bottomLevelPath);

    //    for (int i = 0; i < bottomLevelPath.Count; i++)
    //    {
    //        pathSoFar.Enqueue(bottomLevelPath[i]);
    //    }
    //    return bottomLevelPath;
    //}

    //List<PathFindingNode> updateMidLevelPath(PathFindingNode start, PathFindingNode firstMidLevel, PathFindingNode end) {
    //    List<PathFindingNode> result = new List<PathFindingNode>();
    //    midLevelPath = AStar(firstMidLevel, end);
    //    List<PathFindingNode> bottomLevelPath = new List<PathFindingNode>();
    //    PathFindingNode bottomStart = start;
    //    for (int i = 1; i < midLevelPath.Count; i++)
    //    {
    //        if (bottomLevelPath.Count > 0)
    //        {
    //            bottomStart = bottomLevelPath[bottomLevelPath.Count - 1];
    //        }
    //        bottomLevelPath = UpdateBottomLevelPath(bottomStart, midLevelPath[i]);
    //    }
    //    return result;
    //}

    // do A star 
    // kick out if the parent changes or if you reach end
    private PathFindingNode AStar(PathFindingNode start, PathFindingNode end)
    {
        List<PathFindingNode> path = new List<PathFindingNode>();
        // do A* from start to end but kick out the path if you enter a new region or sub region
        start.CostSoFar = 0;
        PriorityQueue priorityQueue = new PriorityQueue(start);
        Dictionary<string, PathFindingNode> lookUpTable = new Dictionary<string, PathFindingNode>
        {
            { start.GetName, start }
        };
        priorityQueue.Enqueue(start);

        while(priorityQueue.Count() > 0)
        {
            PathFindingNode parent = (PathFindingNode)priorityQueue.Peek();
            priorityQueue.Dequeue();
            foreach(PathFindingNode neighbor in parent.GetNeighbors())
            {
                float costFromParent = (float)Math.Sqrt(Math.Pow(neighbor.GetX - parent.GetX, 2) + Math.Pow(neighbor.GetY - parent.GetY, 2));
                float costSoFar = parent.CostSoFar + costFromParent;
                if(neighbor.GetName == end.GetName)
                {
                    // TODO add hierarchy check to see if we have changed hierarchies. If we have then we should also kick out and return
                    // we made it to the end
                    neighbor.Parent = parent;
                    neighbor.CostSoFar = costSoFar;
                    path.Add(neighbor);
                    return neighbor;
                }
                if(lookUpTable.ContainsKey(neighbor.GetName) && lookUpTable[neighbor.GetName].CostSoFar < costSoFar)
                {
                    continue;
                } else if(!lookUpTable.ContainsKey(neighbor.GetName))
                {
                    neighbor.Parent = parent;
                    neighbor.CostSoFar = costSoFar;
                    lookUpTable.Add(neighbor.GetName, neighbor);
                    priorityQueue.Enqueue(neighbor);
                }
            }
        }

        return null;
        
    }

    /**
    * Search
    * This is a generic search function for the algorithm
    * The start and end nodes must be in the same hierarchy and this will find it's way
    */
    public void Search(INode start, INode end)
    {
        PathFindingNode result = AStar((PathFindingNode)start, (PathFindingNode)end);
        while(result.Parent != null)
        {
            pathSoFar.Enqueue(result);
            result = (PathFindingNode)result.Parent;
        }
        pathVisaulDebugger.DrawLines(pathSoFar.ToList());
        return;
    }

    //public void Search(INode start, INode end, FindNextNode heuristic)
    //{
    //    throw new System.NotImplementedException();
    //}

    public List<INode> GetGraph()
    {
        throw new System.NotImplementedException();
    }
}
