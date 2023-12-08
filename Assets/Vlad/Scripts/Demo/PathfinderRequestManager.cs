using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathfinderRequestManager : MonoBehaviour {
    
    Queue<PathfinderRequest> pathRequests = new Queue<PathfinderRequest>();
    PathfinderRequest currentPathRequest;

    static PathfinderRequestManager instance;

    Pathfinder pathfinder;
    bool isProcessingPath;

    void Awake() {
        instance = this;
        pathfinder = GetComponent<Pathfinder>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathfinderRequest newRequest = new PathfinderRequest(pathStart, pathEnd, callback);
        instance.pathRequests.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessingPath && pathRequests.Count > 0) {
            currentPathRequest = pathRequests.Dequeue();
            isProcessingPath = true;
            pathfinder.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success) {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathfinderRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathfinderRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}