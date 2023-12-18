using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinder : MonoBehaviour
{   
    public enum AlgorithmType {
        AStar_NO_OPT,
        AStart_OPT_1,
        AStar_OPT_2
    }

    public double AveragePathTime {
        get { return activeAlgorithm.GetAverageAllTime(); }
    }

    public Algorithm activeAlgorithm;

    MapGrid grid;
    PathfinderRequestManager requestManager;

    void Awake() {
        activeAlgorithm = new AStarNoOpt();
        grid = GetComponent<MapGrid>();
        requestManager = GetComponent<PathfinderRequestManager>();
    }
    
    public Vector3[] FindPathSync(Vector3 startPos, Vector3 targetPos) {
        List<MapNode> path = activeAlgorithm.FindPath(grid, startPos, targetPos);
        if (path.Count > 0) {
            return SimplifyPath(path);
        } else {
            return new List<Vector3>().ToArray();
        }
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos, activeAlgorithm));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, Algorithm algorithm) {
        List<MapNode> path = algorithm.FindPath(grid, startPos, targetPos);
        yield return null;

        bool pathFound = path.Count > 0;
        Vector3[] waypoints = SimplifyPath(path);
        requestManager.FinishedProcessingPath(waypoints, pathFound);
    }

    Vector3[] SimplifyPath(List<MapNode> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            float dirX = path[i - 1].gridPosition.x - path[i].gridPosition.x;
            float dirY = path[i - 1].gridPosition.y - path[i].gridPosition.y;
            Vector2 directionNew = new Vector2(dirX, dirY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        waypoints.Add(path[path.Count - 1].worldPosition);
        return waypoints.ToArray();
    }
}
