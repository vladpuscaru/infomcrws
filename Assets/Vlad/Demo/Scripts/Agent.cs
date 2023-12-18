using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour {
    public float speed = 20.0f;
    Vector3[] path;
    int targetIndex;

    public bool isOccupied = false;
    public int targetCount = 0;
    public int maxTargets;

    void Start() {
    }

    public void GoTowardsTargetSync(Transform target) {
        isOccupied = true;
        targetCount = targetCount >= maxTargets ? 0 : targetCount + 1;
        targetIndex = 0;
        Vector3[] waypoints = PathfinderRequestManager.RequestPathSync(transform.position, target.position);
        OnPathFound(waypoints, waypoints.Length > 0);
    }

    public void GoTowardsTarget(Transform target) {
        isOccupied = true;
        targetCount = targetCount >= maxTargets ? 0 : targetCount + 1;
        targetIndex = 0;
        PathfinderRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathFound) {
        if (pathFound) {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];

        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    isOccupied = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);
                
                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}