using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;

/**
 * This first Optimisation is related to the way we choose the node
 * with the minimum cost
 * Each time we iterate through the open List and find the minimum
 * To make it more efficient, we make use of a Min Heap data structure
 * 
 */

public class AStar0 : MonoBehaviour
{   
    public Transform seeker, target;

    MyGrid0 grid;

    void Awake() {
        grid = GetComponent<MyGrid0>();
    }

    void Update() {
        if (Input.GetButtonDown("Jump")) {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Node0 startNode = grid.GetNodeFromWorldPoint(startPos);
        Node0 targetNode = grid.GetNodeFromWorldPoint(targetPos);

        /**
         * Location of changes from original
        List<Node0> open = new List<Node0>();
         */

        Heap<Node0> open = new Heap<Node0>(grid.MaxSize);

        HashSet<Node0> closed = new HashSet<Node0>();
        open.Add(startNode);

        while (open.Count > 0) {
            Node0 currentNode = open.RemoveFirst();

            closed.Add(currentNode);

            if (currentNode == targetNode) {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + "ms");
                grid.open = open.ToList();
                grid.closed = closed.ToList();
                RetracePath(startNode, targetNode);
                return;
            }

            List<Node0> neighbours = grid.GetNodeNeighbours(currentNode);
            foreach (Node0 neighbour in neighbours) {
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

    void RetracePath(Node0 startNode, Node0 endNode) {
        List<Node0> path = new List<Node0>();
        Node0 currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node0 nodeA, Node0 nodeB) {
        int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);            
        }
        return 14 * distX + 10 * (distY - distX);            
    }
}
