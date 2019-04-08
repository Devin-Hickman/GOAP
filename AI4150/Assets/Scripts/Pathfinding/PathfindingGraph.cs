using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingGraph : MonoBehaviour, IGraph
{
    private Queue<PathFindingNode> pathSoFar = new Queue<PathFindingNode>();
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

    private void Awake()
    {
        hierarchicalGrids = new PathFindingNode[tileMapRegions.Count][,];
    }

    void Start()
    {
        BuildGraph();
    }

    private PathFindingNode[,] CreateLowLevelGrid()
    {
        int numCols = (int)Mathf.Abs((gridEndPos.position.x - gridStartPos.position.x) / gridCellSize);
        int numRows = (int)Mathf.Abs((gridEndPos.position.y - gridStartPos.position.y) / gridCellSize);
        return new PathFindingNode[numCols, numRows];
    }

    public List<INode> BuildGraph()
    {
        List<INode> res = new List<INode>();
        BuildPathfindingNodes(0);
        return res;
    }

    private void BuildPathfindingNodes(int currentLevel)
    {
        int xIndex = -1;
        int yIndex = -1;

        hierarchicalGrids[currentLevel] = CreateLowLevelGrid();
        int nodesMade = 0;

        for (int x = (int)gridStartPos.position.x; x < gridEndPos.position.x; x += gridCellSize)
        {
            xIndex++;
            Debug.Log(xIndex);
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
                yIndex++;
                hierarchicalGrids[currentLevel][xIndex, yIndex] = tmp;
            }
        }
        AssignNodeNeighbors();
    }

    private void CreateDebugNode(int x, int y, bool walkable, string name)
    {
        GameObject g = new GameObject();
        g.AddComponent<NodeGizmos>();
        g.GetComponent<NodeGizmos>().SetValues(x, y, walkable, name);
    }

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
    private PathFindingNode returnNodeWithShortestDistToTarget(PathFindingNode start, PathFindingNode end)
    {
        PathFindingNode res = null;

        return res;
    }

    //public void search(PathfindingNode start, PathfindingNode end)
    //{
    //    findNextNode heuristic = returnNodeWithShortestDistToTarget;
    //    privateSearch(start, end, heuristic);

    //}

    /**
    * Search
    * The nodes it is passed must be bottom level nodes if they are regional or parent nodes then this won't work... 
    * These bottom level nodes most
    * 
    */
    // this assumes there is always a path to a given location
    public void search(PathFindingNode start, PathFindingNode end)
    {
        if(start.GetParentRegion().GetParentRegion() == null || end.GetParentRegion().GetParentRegion() == null)
        {
            //Is this true? What about searching on the top-most level?
            throw new System.Exception("start and end nodes must both have two layers of parents above them.");
        }

        if(!start.GetParentRegion().GetParentRegion().Equals(end.GetParentRegion().GetParentRegion()))
        {
            // generate the toplevel path then 
            topLevelPath = aStar(start.GetParentRegion().GetParentRegion(), end.GetParentRegion().GetParentRegion());
            for (int i = 1; i < topLevelPath.Count; i++)
            {
                // initi
                PathFindingNode midLevelStart = start.GetParentRegion();
                if (midLevelPath.Count > 0)
                {
                    midLevelStart = midLevelPath[midLevelPath.Count - 1];
                }
                updateMidLevelPath(start, midLevelStart, topLevelPath[i]);
            }
        } else if (!start.GetParentRegion().Equals(end.GetParentRegion()))
        {
            // do the mid level Path
            updateMidLevelPath(start, start.GetParentRegion(), end.GetParentRegion());
        } else
        {
            // do straight A* between the nodes and return that path.
            updateBottomLevelPath(start, end);
        }
    }

    List<PathFindingNode> updateBottomLevelPath(PathFindingNode start, PathFindingNode end)
    {
        List<PathFindingNode> bottomLevelPath = aStar(start, end);
        for (int i = 0; i < bottomLevelPath.Count; i++)
        {
            pathSoFar.Enqueue(bottomLevelPath[i]);
        }
        return bottomLevelPath;
    }

    List<PathFindingNode> updateMidLevelPath(PathFindingNode start, PathFindingNode firstMidLevel, PathFindingNode end) {
        List<PathFindingNode> result = new List<PathFindingNode>();
        midLevelPath = aStar(firstMidLevel, end);
        List<PathFindingNode> bottomLevelPath = new List<PathFindingNode>();
        PathFindingNode bottomStart = start;
        for (int i = 1; i < midLevelPath.Count; i++)
        {
            if (bottomLevelPath.Count > 0)
            {
                bottomStart = bottomLevelPath[bottomLevelPath.Count - 1];
            }
            bottomLevelPath = updateBottomLevelPath(bottomStart, midLevelPath[i]);
        }
        return result;
    }

    // do A star 
    // kick out if the parent changes or if you reach end
    private List<PathFindingNode> aStar(PathFindingNode start, PathFindingNode end)
    {
        List<PathFindingNode> path = new List<PathFindingNode>();
        // do A* from start to end but kick out the path if you enter a new region or sub region

        return path;
        
    }

    public void Search(INode start, INode end)
    {
        throw new System.NotImplementedException();
    }

    public void Search(INode start, INode end, FindNextNode heuristic)
    {
        throw new System.NotImplementedException();
    }

    public List<INode> GetGraph()
    {
        throw new System.NotImplementedException();
    }
}
