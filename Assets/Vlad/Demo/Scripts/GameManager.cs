
using System.Collections.Generic;
using System;
using UnityEngine;

namespace InfomCRWS {
    public class GameManager : MonoBehaviour {
        
        public MapGrid m_grid;
        public Vector2 m_gridSize;

        public bool m_displayGizmos;

        public GameObject m_agentPrefab;
        public Transform m_agentSpawnPoint;
        public int m_agentsToBeSpawned;
        public List<Agent> m_agents;
        public List<Transform> m_targets;

        void Start() {
            int targetCount = 0;

            System.Random r = new System.Random();
            float minSpeed = 20.0f;
            float maxSpeed = 50.0f;
            for (int i = 0; i < m_agentsToBeSpawned; i++) {
                Agent newAgent = Instantiate(m_agentPrefab, m_agentSpawnPoint).GetComponent<Agent>();
                newAgent.targetCount = targetCount;
                newAgent.speed = minSpeed + (float)r.NextDouble() * maxSpeed;
                m_agents.Add(newAgent);

                targetCount++;
                targetCount = targetCount >= m_targets.Count ? 0 : targetCount;
            }
        }

        void Update() {
            foreach (Agent a in m_agents) {
                if (!a.isOccupied) {
                    a.targetCount = a.targetCount >= m_targets.Count ? 0 : a.targetCount;
                    Transform nextTarget = m_targets[a.targetCount];
                    a.GoTowardsTarget(nextTarget);
                }
            }
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