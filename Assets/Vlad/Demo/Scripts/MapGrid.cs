using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapGrid : MonoBehaviour
{   
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float NodeRadius;
    public MapNode[,] grid;

    public float NodeDiameter;
    public int gridSizeX, gridSizeY;

    void Awake() {
        NodeDiameter = NodeRadius*2;

        gridWorldSize = new Vector2(Mathf.Clamp(gridWorldSize.x, 0, gridWorldSize.x), Mathf.Clamp(gridWorldSize.y, 0, gridWorldSize.y));
        CreateGrid();
    }

    void Update() {
        if (gridWorldSize.x * gridWorldSize.y != grid.LongLength) {
            CreateGrid();
        }
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid() {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / NodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / NodeDiameter);

        grid = new MapNode[gridSizeX, gridSizeY];
        
        Vector3 worldBottomLeft = transform.position + Vector3.left * gridWorldSize.x / 2 + Vector3.back * gridWorldSize.y / 2; // using vector3.back bcs y is actually Z in our world

        for (int i = 0; i < gridSizeX; i++) {
            for (int j = 0; j < gridSizeY; j++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * NodeDiameter + NodeRadius) + Vector3.forward * (j * NodeDiameter + NodeRadius);
                Vector2Int gridPosition = new Vector2Int(i, j);
                bool walkable = !(Physics.CheckSphere(worldPoint, NodeRadius, unwalkableMask));
                grid[i, j] = new MapNode(walkable, worldPoint, gridPosition);
            }
        }
    }

    public MapNode GetNodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; // again, the world Z is our Y in code
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<MapNode> GetNodeNeighbours(MapNode Node2) {
        List<MapNode> neighbours = new List<MapNode>();

        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }

                int checkX = Node2.gridPosition.x + i;
                int checkY = Node2.gridPosition.y + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}
