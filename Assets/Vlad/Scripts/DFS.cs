using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DFS : MonoBehaviour
{   
    public Transform seeker, target;

    MyGrid grid;

    void Awake() {
        grid = GetComponent<MyGrid>();
    }

    void Update() {
        if (grid.activeAlg == AlgorithmType.DFS && seeker && target) {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        Stack<Node> open = new Stack<Node>();
        HashSet<Node> closed = new HashSet<Node>();

        open.Push(startNode);

        while (open.Count > 0) {
            if (Time.realtimeSinceStartup > 5) {
                grid.open = open.ToList();
                grid.closed = closed.ToList();
                RetracePath(startNode, targetNode);
                return;
            }

            Node currentNode = open.Pop();

            closed.Add(currentNode);

            if (currentNode == targetNode) {
                grid.open = open.ToList();
                grid.closed = closed.ToList();
                RetracePath(startNode, targetNode);
                return;
            }

            List<Node> neighbours = grid.GetNodeNeighbours(currentNode);
            foreach (Node neighbour in neighbours) {
                if (!neighbour.walkable || closed.Contains(neighbour)) {
                    continue;
                }

                neighbour.parent = currentNode;
                open.Push(neighbour);
            }
        }
    }

    void RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode && currentNode != null) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
    }
}
