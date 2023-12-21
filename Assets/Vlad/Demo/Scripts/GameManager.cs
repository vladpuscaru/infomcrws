
using System.Collections.Generic;
using System;
using UnityEngine;

namespace InfomCRWS {
    public class GameManager : MonoBehaviour {

        public struct GameStats {
            public int noAgents;
            public Pathfinder.AlgorithmType algorithm;
            public double avgPathTime;
            public int noCells;
        }

        public Pathfinder m_pathfinder;

        GameStats stats;
        public GameStats Stats {
            get { return stats; }
        }

        public MapGrid m_grid;

        public bool m_displayGizmos;

        public GameObject m_agentPrefab;
        public List<Transform> m_agentSpawnPoints;
        public int m_agentsToBeSpawned;
        public List<Agent> m_agents;
        public List<Transform> m_targets;

        int m_targetCount = 0;

        void Start() {
            for (int i = 0; i < m_agentsToBeSpawned; i++) {
                spawnAgent(PickRandomSpawnPoint());
            }
        }

        void Update() {
            foreach (Agent a in m_agents) {
                if (a != null && !a.isOccupied) {
                    a.targetCount = a.targetCount >= m_targets.Count ? 0 : a.targetCount;
                    Transform nextTarget = PickRandomTargetPoint();
                    a.GoTowardsTarget(nextTarget);
                    //a.GoTowardsTargetSync(nextTarget);
                }
            }

            UpdateStats();
        }

        Transform PickRandomTargetPoint() {
            System.Random rand = new System.Random();
            int randIdx = rand.Next(0, m_targets.Count);
            return m_targets[randIdx];
        }

        Vector3 PickRandomSpawnPoint() {
            System.Random rand = new System.Random();
            int randIdx = rand.Next(0, m_agentSpawnPoints.Count);
            return m_agentSpawnPoints[randIdx].transform.position;
        }

        void UpdateStats() {
            stats.noAgents = m_agents.Count;
            stats.noCells = Convert.ToInt32(m_grid.gridSizeX * m_grid.gridSizeY);
            stats.algorithm = Pathfinder.AlgorithmType.AStar_NO_OPT;
            stats.avgPathTime = m_pathfinder.AveragePathTime;
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


        void spawnAgent(Vector3 position) {
            System.Random r = new System.Random();
            float minSpeed = 20.0f;
            float maxSpeed = 50.0f;

            Agent newAgent = Instantiate(m_agentPrefab, position, Quaternion.identity).GetComponent<Agent>();
            newAgent.targetCount = m_targetCount;
            newAgent.maxTargets = m_targets.Count;
            newAgent.speed = minSpeed + (float)r.NextDouble() * maxSpeed;

            m_agents.Add(newAgent);

            m_targetCount++;
            m_targetCount = m_targetCount >= m_targets.Count ? 0 : m_targetCount;
        }


        public void modifyNoAgents(int targetValue) {
            if (targetValue <= 0) {
                foreach(Agent a in m_agents) {
                    Destroy(a.gameObject);
                }
                m_agents.Clear();
            }

            if (targetValue < m_agents.Count) {
                int target = m_agents.Count - targetValue;
                for (int i = 0; i < target; i++) {
                    Destroy(m_agents[0].gameObject);
                    m_agents.RemoveAt(0);
                }
            } else if (targetValue > m_agents.Count) {
                int target = targetValue - m_agents.Count;
                for (int i = 0; i < target; i++) {
                    spawnAgent(PickRandomSpawnPoint());
                }
            }
        }
    }
}