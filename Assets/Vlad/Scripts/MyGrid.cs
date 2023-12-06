using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyGrid : MonoBehaviour
{   
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;

    public float nodeDiameter;
    public int gridSizeX, gridSizeY;

    public AlgorithmType activeAlg;
    public List<Node> open;
    public List<Node> closed;
    public List<Node> path;

    void Start() {
        nodeDiameter = nodeRadius*2;

        gridWorldSize = new Vector2(Mathf.Clamp(gridWorldSize.x, 0, gridWorldSize.x), Mathf.Clamp(gridWorldSize.y, 0, gridWorldSize.y));
        CreateGrid();
    }

    void Update() {
        if (gridWorldSize.x * gridWorldSize.y != grid.LongLength) {
            CreateGrid();
        }
    }

    void CreateGrid() {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        grid = new Node[gridSizeX, gridSizeY];
        
        Vector3 worldBottomLeft = transform.position + Vector3.left * gridWorldSize.x / 2 + Vector3.back * gridWorldSize.y / 2; // using vector3.back bcs y is actually Z in our world

        for (int i = 0; i < gridSizeX; i++) {
            for (int j = 0; j < gridSizeY; j++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                Vector2Int gridPosition = new Vector2Int(i, j);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[i, j] = new Node(walkable, worldPoint, gridPosition);
            }
        }
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; // again, the world Z is our Y in code
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> GetNodeNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }

                int checkX = node.gridPosition.x + i;
                int checkY = node.gridPosition.y + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}
