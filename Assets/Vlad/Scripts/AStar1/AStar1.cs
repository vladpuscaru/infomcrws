using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;

public class AStar1 : MonoBehaviour
{   
    MyGrid1 grid;
    PathRequestManager requestManager;

    void Awake() {
        grid = GetComponent<MyGrid1>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathFound = false;

        Node1 startNode = grid.GetNodeFromWorldPoint(startPos);
        Node1 targetNode = grid.GetNodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable) {
            Heap<Node1> open = new Heap<Node1>(grid.MaxSize);

            HashSet<Node1> closed = new HashSet<Node1>();
            open.Add(startNode);

            while (open.Count > 0) {
                Node1 currentNode = open.RemoveFirst();

                closed.Add(currentNode);

                if (currentNode == targetNode) {
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + "ms");
                    pathFound = true;
                    break;
                }

                List<Node1> neighbours = grid.GetNodeNeighbours(currentNode);
                foreach (Node1 neighbour in neighbours) {
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
                        }
                    }
                }
            }
        }
        yield return null;

        if (pathFound) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathFound);
    }

    Vector3[] RetracePath(Node1 startNode, Node1 endNode) {
        List<Node1> path = new List<Node1>();
        Node1 currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        Vector3[] waypoints = SimplifyPath(path);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node1> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            float dirX = path[i - 1].gridPosition.x - path[i].gridPosition.x;
            float dirY = path[i - 1].gridPosition.y - path[i].gridPosition.y;
            Vector2 directionNew = new Vector2(dirX, dirY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        waypoints.Add(path[path.Count - 1].worldPosition);
        return waypoints.ToArray();
    }

    int GetDistance(Node1 nodeA, Node1 nodeB) {
        int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);            
        }
        return 14 * distX + 10 * (distY - distX);            
    }
}
