using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Node0 : IHeapItem<Node0>
{   
    public bool walkable;
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    public int gCost;
    public int hCost;
    public Node0 parent;
    int heapIndex;

    public Node0(bool _walkable, Vector3 _worldPosition, Vector2Int _gridPosition) {
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

    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    public int CompareTo(Node0 other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0) {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
