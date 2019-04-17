using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class PathfindingGraph : MonoBehaviour, IGraph
{
    public List<AbstractNode> PathSoFar { get; set; }
    private Dictionary<string, PathFindingNode> bottomLevelDict = new Dictionary<string, PathFindingNode>();
    private Dictionary<string, ParentRegionNode> midLevelDict = new Dictionary<string, ParentRegionNode>();
    private Dictionary<string, ParentRegionNode> topLevelDict = new Dictionary<string, ParentRegionNode>();
    //List<PathFindingNode> topLevelPath = new List<PathFindingNode>();
    //List<PathFindingNode> midLevelPath = new List<PathFindingNode>();

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
        PathSoFar = new List<AbstractNode>();
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
        float tmpX = Mathf.Round(x);
        float tmpY = Mathf.Round(y);

        x = (tmpX > x) ? Mathf.Floor(x) + 0.5f : Mathf.Ceil(x) - 0.5f;
        y = (tmpY > y) ? Mathf.Floor(y) + 0.5f : Mathf.Ceil(y) - 0.5f;

        string key = string.Format("{0:N1}", x + "," + string.Format("{0:N1}", y));
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
    public void BuildGraph()
    {
        BuildPathfindingNodes();
    }

    /*
     * Iteater over entire low-level grid
     * Iterate over n grid + 1 tileMaps (List<TileMaps>)
     * Check to see if Tilemap t contains Tile at point x,y
     * */

    /**
     * Builds the lowest level of the graph using the tiles and updates the hierarchicalGrids object with that lowest level
     */
    private void BuildPathfindingNodes()
    {
        int xIndex = -1;
        int yIndex = -1;

        hierarchicalGrids[0] = CreateLowLevelGrid();
        int nodesMade = 0;

        for (float x = gridStartPos.position.x; x < gridEndPos.position.x; x += gridCellSize)
        {
            xIndex++;
            yIndex = -1;
            for (float y = gridStartPos.position.y; y < gridEndPos.position.y; y += gridCellSize)
            {
                PathFindingNode tmp = null;
                TileBase tileBase = tileMapRegions[0].GetTile(new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0));
                TileBase parent1TileBase = tileMapRegions[1].GetTile(new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0));
                TileBase parent2TileBase = tileMapRegions[2].GetTile(new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0));

                if (tileBase != null) // Tile exists
                {
                    nodesMade++;
                    TileBase obstacleBase = obstacleMap.GetTile(new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0));
                    bool walkable = (obstacleBase == null) ? true : false;
                    tmp = new PathFindingNode(new Vector2(x, y), walkable);
                    CreateDebugNode(x, y, walkable, "L" + 0 + " N" +nodesMade);
                }
                else
                {
                    TileBase obstacleBase = obstacleMap.GetTile(new Vector3Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y), 0));
                    if(obstacleBase != null)
                    {
                        nodesMade++;
                        tmp = new PathFindingNode(new Vector2(x, y), false);
                        CreateDebugNode(x, y, false, "L" + 0 + " N" + nodesMade);
                    }
                }
                yIndex++;
                if(tmp != null)
                {
                    bottomLevelDict.Add(tmp.GetName, tmp);
                }
                if (tmp != null && parent1TileBase != null && parent2TileBase != null)
                {
                    string parent1Sprite = ((Tile)parent1TileBase).sprite.name;
                    if (!midLevelDict.ContainsKey(parent1Sprite))
                    {
                        midLevelDict[parent1Sprite] = new ParentRegionNode(parent1Sprite);
                    }
                    midLevelDict[parent1Sprite].AddToChildNodes(tmp);
                    tmp.ParentRegion = midLevelDict[parent1Sprite];

                    string parent2Sprite = ((Tile)parent2TileBase).sprite.name;
                    if (!topLevelDict.ContainsKey(parent2Sprite))
                    {
                        topLevelDict[parent2Sprite] = new ParentRegionNode(parent2Sprite);
                    }
                    topLevelDict[parent2Sprite].AddToChildNodes(midLevelDict[parent1Sprite]);
                    midLevelDict[parent1Sprite].ParentRegion = topLevelDict[parent2Sprite];
                }
                hierarchicalGrids[0][xIndex, yIndex] = tmp;
            }
        }
        // right now the mid and top level nodes don't have x and y values. Have them make their own based on
        // child nodes
        foreach (KeyValuePair<string, ParentRegionNode> entry in midLevelDict)
        {
            entry.Value.CalculatePos();
        }
        foreach (KeyValuePair<string, ParentRegionNode> entry in topLevelDict)
        {
            entry.Value.CalculatePos();
        }
        AssignNodeNeighbors();
    }

    private void UpdateMidAndTopLevel(int x, int y, PathFindingNode curNode, PathFindingNode[,] grid, string currentParent, string currentParentsParent)
    {
        if (currentParent != null && grid[x, y].ParentRegion != null)
        {
            string otherSprite = grid[x, y].ParentRegion.TileSprite;
            if (!currentParent.Equals(otherSprite))
            {
                curNode.ParentRegion.AddToBorderNodes(grid[x, y], otherSprite, grid[x, y].ParentRegion);
            }

            if (currentParentsParent != null && grid[x, y].ParentRegion.ParentRegion != null)
            {
                string otherSprite2 = grid[x, y].ParentRegion.ParentRegion.TileSprite;
                if (!currentParentsParent.Equals(otherSprite2))
                {
                    curNode.ParentRegion.ParentRegion.AddToBorderNodes(grid[x, y].ParentRegion, otherSprite2, grid[x, y].ParentRegion.ParentRegion);
                }
            }
        }
    }

    /**
    * Creates the nodes that are visible to the developer
    * 
    */
    private void CreateDebugNode(float x, float y, bool walkable, string name)
    {
        GameObject g = new GameObject();
        g.transform.parent = GameObject.Find("DebugNodes").transform;
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
                            string currentParent = null;
                            string currentParentsParent = null;
                            if (curNode.ParentRegion != null) {
                                currentParent = curNode.ParentRegion.TileSprite;
                                if(curNode.ParentRegion.ParentRegion != null)
                                {
                                    currentParentsParent = curNode.ParentRegion.ParentRegion.TileSprite;
                                }
                            }

                            int xTmp = x - 1;
                            if (xTmp > 0 && grid[xTmp, y] != null && grid[xTmp, y].IsWalkable) {
                                curNode.AddNeighbor(grid[xTmp, y]);
                                UpdateMidAndTopLevel(xTmp, y, curNode, grid, currentParent, currentParentsParent);
                            }

                            xTmp = x + 1;
                            if (xTmp < grid.GetLength(0) && grid[xTmp, y] != null && grid[xTmp, y].IsWalkable) {
                                curNode.AddNeighbor(grid[xTmp, y]);
                                UpdateMidAndTopLevel(xTmp, y, curNode, grid, currentParent, currentParentsParent);
                            }

                            int yTmp = y - 1;
                            if (yTmp > 0 && grid[x, yTmp] != null && grid[x, yTmp].IsWalkable) {
                                curNode.AddNeighbor(grid[x, yTmp]);
                                UpdateMidAndTopLevel(x, yTmp, curNode, grid, currentParent, currentParentsParent);
                            }

                            yTmp = y + 1;
                            if (yTmp < grid.GetLength(1) && grid[x, yTmp] != null && grid[x, yTmp].IsWalkable) {
                                curNode.AddNeighbor(grid[x, yTmp]);
                                UpdateMidAndTopLevel(x, yTmp, curNode, grid, currentParent, currentParentsParent);
                            }

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
    private AbstractNode AStar(AbstractNode start, AbstractNode end)
    {
        List<AbstractNode> path = new List<AbstractNode>();
        // do A* from start to end but kick out the path if you enter a new region or sub region
        start.CostSoFar = 0;
        PriorityQueue priorityQueue = new PriorityQueue(start);
        Dictionary<string, AbstractNode> lookUpTable = new Dictionary<string, AbstractNode>
        {
            { start.GetName, start }
        };
        priorityQueue.Enqueue(start);

        while(priorityQueue.Count() > 0)
        {
            AbstractNode parent = (AbstractNode)priorityQueue.Peek();
            priorityQueue.Dequeue();
            foreach(AbstractNode neighbor in parent.GetNeighbors())
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
                } else
                {
                    lookUpTable[neighbor.GetName].CostSoFar = costSoFar;
                    if(!priorityQueue.IsConsistent())
                    {

                    }
                }
            }
        }

        return null;
        
    }

    /**
    * Search
    * This is a generic search function for the algorithm
    * The start and end nodes must be in the same hierarchy and this will find it's way
    * 
    * PSEUDOCODE:
    * Check if parent regions are the same and are not neighbors
    * if not check if those parents have the same parents
    * if top level parents not the same and are not neighbors:
    * A* start.parentRegion.parentRegion, end.parentRegion.parentRegion
    * Collect the path
    * if the top level parents are the same or neighbors just start here
    * get closest border node of next region in top level heirarchy
    * Do A* in mid level heirarchy from start.ParentRegion to found border node
    * Collect the path
    * Get the closest node of the next region in mid level heirarchy
    * if mid level parents are the same or neighbors just start here.
    * Do A* in bottom level heirarchy from start to found border node
    * append the path
    * Loop on all of the above until we make it or we find the end point is unreachable.
    * 
    */
    public void Search(AbstractNode start, AbstractNode end)
    {
        if(start == null || end == null || !((PathFindingNode)start).IsWalkable || !((PathFindingNode)end).IsWalkable)
        {
            // invalid node sent
            return;
        }
        PathSoFar = new List<AbstractNode>();
        // midPathSoFar is defined as an abstract node for flexibility but should only contain ParentRegionNodes
        List<AbstractNode> MidPathSoFar = new List<AbstractNode>();
        if(start.ParentRegion != null && end.ParentRegion != null && !start.ParentRegion.Equals(end.ParentRegion))
        {
            // If the parent regions are neighbors then we can just do A* start to end without worrying about heirarchies
            List<INode> neighbors = start.ParentRegion.GetNeighbors();
            bool found = false;
            for (int i = 0; i < neighbors.Count; i++)
            {
                if(neighbors[i].Equals(end.ParentRegion))
                {
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                // do A* in the mid level heirarchy
                AbstractNode midResult = AStar(start.ParentRegion, end.ParentRegion);
                MidPathSoFar.AddRange(GeneratePathFromParents(midResult));
                pathVisaulDebugger.DrawLines(MidPathSoFar);
            }
        }
        AbstractNode tmpStart = start;
        AbstractNode tmpEnd = end;
        int j = 0;
        while (j + 1 < MidPathSoFar.Count)
        // While the next element in midLevel path isn't the last 
        // get the closest border node of the next parent region then do A* to that border node
        {
            AbstractNode closestNode = ((ParentRegionNode)MidPathSoFar[j]).GetClosestBorderNode(tmpStart.Pos, ((ParentRegionNode)MidPathSoFar[j+1]).TileSprite);
            AbstractNode tmpResult = AStar((PathFindingNode)tmpStart, (PathFindingNode)closestNode);
            if (tmpResult == null)
            {
                // no path found stop path finding
                return;
            }
            PathSoFar.AddRange(GeneratePathFromParents(tmpResult));
            tmpStart = closestNode;
            j++;
        }
        // after the loop regardless of if it has executed 0 times or a hundred
        // do A* from either the last border node it went to or the starting node to the end
        AbstractNode result = AStar((PathFindingNode)tmpStart, (PathFindingNode)end);
        if (result == null)
        {
            // No path was found.
            return;
        }
        PathSoFar.AddRange(GeneratePathFromParents(result));
        pathVisaulDebugger.DrawLines(PathSoFar);
        return;
    }

    public List<INode> GetGraph()
    {
        throw new System.NotImplementedException();
    }

    private List<AbstractNode> GeneratePathFromParents(AbstractNode result)
    {
        List<AbstractNode> path = new List<AbstractNode>();
        int count = 0;
        while (result.Parent != null)
        {
            if (result.CostSoFar == 0)
            {
                // the result node has a cost of zero we made it back
                break;
            }
            path.Insert(0, result);
            result = (AbstractNode)result.Parent;
            count++;
        }
        return path;
    }

}

