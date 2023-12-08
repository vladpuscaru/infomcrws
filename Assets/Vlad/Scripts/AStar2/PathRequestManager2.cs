using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathRequestManager2 : MonoBehaviour {
    
    Queue<PathRequest> pathRequests = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager2 instance;

    AStar2 pathfinding;
    bool isProcessingPath;

    void Awake() {
        instance = this;
        pathfinding = GetComponent<AStar2>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequests.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessingPath && pathRequests.Count > 0) {
            currentPathRequest = pathRequests.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success) {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}