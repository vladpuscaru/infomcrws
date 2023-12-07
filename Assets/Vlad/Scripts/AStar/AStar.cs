using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : MonoBehaviour
{   
    public Transform seeker, target;

    MyGrid grid;

    void Awake() {
        grid = GetComponent<MyGrid>();
    }

    void Update() {
        if (grid.activeAlg == AlgorithmType.ASTAR && seeker && target) {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        List<Node> open = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();

        open.Add(startNode);

        while (open.Count > 0) {
            Node currentNode = open[0];
            for (int i = 1; i < open.Count; i++) {
                if (
                currentNode.FCost > open[i].FCost || 
                (currentNode.FCost == open[i].FCost && currentNode.hCost > open[i].hCost)
                ) {
                    currentNode = open[i];
                }
            }

            open.Remove(currentNode);
            closed.Add(currentNode);

            if (currentNode == targetNode) {
                grid.open = open;
                grid.closed = closed.ToList();
                RetracePath(startNode, targetNode);
                return;
            }

            List<Node> neighbours = grid.GetNodeNeighbours(currentNode);
            foreach (Node neighbour in neighbours) {
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

    void RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB) {
        int distX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int distY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (distX > distY) {
            return 14 * distY + 10 * (distX - distY);            
        }
        return 14 * distX + 10 * (distY - distX);            
    }
}
