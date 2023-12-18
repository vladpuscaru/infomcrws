using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

public class AStarNoOpt : Algorithm
{
    List<double> pathTimes = new List<double>();

    List<MapNode> Algorithm.FindPath(MapGrid grid, Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        bool pathFound = false;

        MapNode startNode = grid.GetNodeFromWorldPoint(startPos);
        MapNode targetNode = grid.GetNodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable) {
            Heap<MapNode> open = new Heap<MapNode>(grid.MaxSize);

            HashSet<MapNode> closed = new HashSet<MapNode>();
            open.Add(startNode);

            while (open.Count > 0) {
                MapNode currentNode = open.RemoveFirst();

                closed.Add(currentNode);

                if (currentNode == targetNode) {
                    sw.Stop();
                    pathTimes.Add(sw.ElapsedMilliseconds);
                    pathFound = true;
                    break;
                }

                List<MapNode> neighbours = grid.GetNodeNeighbours(currentNode);
                foreach (MapNode neighbour in neighbours) {
                    if (!neighbour.walkable || closed.Contains(neighbour)) {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !open.Contains(neighbour)) {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!open.Contains(neighbour)) {
                            open.Add(neighbour);
                        } else {
                            open.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        if (pathFound) {
            return RetracePath(startNode, targetNode);
        } else {
            return new List<MapNode>();
        }
    }

    List<MapNode> RetracePath(MapNode startNode, MapNode endNode) {
        List<MapNode> path = new List<MapNode>();
        MapNode currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    int GetDistance(MapNode nodeA, MapNode nodeB) {
        int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);            
        }
        return 14 * distX + 10 * (distY - distX);            
    }

    public double GetAverageAllTime()
    {
        return pathTimes.Average();
    }
}