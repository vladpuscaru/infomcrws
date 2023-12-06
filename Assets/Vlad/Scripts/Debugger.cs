using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{   

    public MyGrid grid;

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(grid.gridWorldSize.x, 1, grid.gridWorldSize.y));

        if (grid != null && grid.grid != null) {
            foreach (Node n in grid.grid) {
                Gizmos.color = n.walkable ? Color.white : Color.red;
                    if (grid.open != null && grid.open.Contains(n)) {
                        Gizmos.color = Color.cyan;
                    }
                    if (grid.closed != null && grid.closed.Contains(n)) {
                        Gizmos.color = Color.blue;
                    }
                    if (grid.path != null && grid.path.Contains(n)) {
                        Gizmos.color = Color.black;
                    }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (grid.nodeDiameter - .1f));
            }
        }
    }

}
