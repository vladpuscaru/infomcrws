using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{   
    public bool walkable;
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool _walkable, Vector3 _worldPosition, Vector2Int _gridPosition) {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridPosition = _gridPosition;
        parent = null;
    }

    public int FCost {
        get {
            return gCost * hCost;
        }
    }
}
