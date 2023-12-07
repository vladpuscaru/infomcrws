using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger1 : MonoBehaviour
{   

    public MyGrid1 grid;
    public bool displayGridGizmos;

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(grid.gridWorldSize.x, 1, grid.gridWorldSize.y));

        if (grid != null && grid.grid != null && displayGridGizmos) {
            foreach (Node1 n in grid.grid) {
                Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (grid.NodeDiameter - .1f));
            }
        }
    }

}
