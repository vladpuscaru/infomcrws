
using UnityEngine;

namespace InfomCRWS {
    public class GameManager : MonoBehaviour {
        
        public MapGrid m_grid;
        public Vector2 m_gridSize;

        public int m_agents;

        public bool m_displayGizmos;

        void Start() {
            Debug.Log("");
        }

        void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, new Vector3(m_grid.gridWorldSize.x, 1, m_grid.gridWorldSize.y));

            if (m_grid != null && m_grid.grid != null && m_displayGizmos) {
                foreach (MapNode n in m_grid.grid) {
                    Gizmos.color = n.walkable ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (m_grid.NodeDiameter - .1f));
                }
            }
        }


        void spawnAgent(Transform position) {

        }
    }
}