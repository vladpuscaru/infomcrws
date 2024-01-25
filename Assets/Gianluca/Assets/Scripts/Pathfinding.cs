using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Diagnostics;
using System;
using TMPro; 
public class PathfindingGP : MonoBehaviour
{

    GridGP grid;
    public TextMeshProUGUI pathfindingTimeText; // Assign this in the inspector


    void Awake()
    {
        grid = GetComponent<GridGP>();
    }


    public void FindPath(PathRequest request, Action<PathResult> callback)
    {

        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;



        NodeGP startNode = grid.NodeFromWorldPoint(request.pathStart);
        NodeGP targetNode = grid.NodeFromWorldPoint(request.pathEnd);

        if (startNode.walkable && targetNode.walkable)
        {
            HeapGP<NodeGP> openSet = new HeapGP<NodeGP>(grid.MaxSize);
            HashSet<NodeGP> closedSet = new HashSet<NodeGP>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                NodeGP node = openSet.RemoveFirst();

                closedSet.Add(node);

                if (node == targetNode)
                {
                    pathSuccess = true;

                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    DisplayPathfindingTime(sw.ElapsedMilliseconds);
                    break;
                }

                foreach (NodeGP neighbour in grid.GetNeighbours(node))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new PathResult(waypoints, pathSuccess, request.callback));
    }

    private void DisplayPathfindingTime(long milliseconds)
    {
        if (pathfindingTimeText != null)
        {
            pathfindingTimeText.text = "Path found: " + milliseconds + " ms";
        }
    }

    Vector3[] RetracePath(NodeGP startNode, NodeGP endNode)
    {
        List<NodeGP> path = new List<NodeGP>();
        NodeGP currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<NodeGP> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(NodeGP nodeA, NodeGP nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
