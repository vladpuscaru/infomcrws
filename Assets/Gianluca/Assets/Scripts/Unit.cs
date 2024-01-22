using UnityEngine;
using System.Collections;

public class UnitGP : MonoBehaviour
{
    public Transform target;
    float speed = 20;
    Vector3[] path;
    int targetIndex;
    Coroutine followPathCoroutine; // Reference to the FollowPath coroutine

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned on " + gameObject.name);
            return;
        }
        PathRequestManagerGP.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            if (followPathCoroutine != null) // Safely stop the existing coroutine if it's running
            {
                StopCoroutine(followPathCoroutine);
            }
            followPathCoroutine = StartCoroutine(FollowPath()); // Start the FollowPath coroutine and keep a reference
        }
    }

    IEnumerator FollowPath()
    {
        if (path == null || path.Length == 0)
        {
            Debug.LogError("Received an empty path");
            yield break;
        }

        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break; // End the coroutine if the end of the path is reached
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
