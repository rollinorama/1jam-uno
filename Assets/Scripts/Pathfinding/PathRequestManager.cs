using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.Path
{
    public class PathRequestManager : MonoBehaviour
    {
        private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        private PathRequest _currentPathRequest;
        private Pathfinding _pathfinding;

        private bool _isProcessingPath;

        static PathRequestManager _instance;

        private void Awake()
        {
            _instance = this;
            _pathfinding = GetComponent<Pathfinding>();
        }


        public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            _instance.pathRequestQueue.Enqueue(newRequest);
            _instance.TryProcessNext();
        }

        private void TryProcessNext()
        {
            if (!_isProcessingPath && pathRequestQueue.Count > 0)
            {
                _currentPathRequest = pathRequestQueue.Dequeue();
                _isProcessingPath = true;
                _pathfinding.StartFindPath(_currentPathRequest.pathStart, _currentPathRequest.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector2[] path, bool success)
        {
            _currentPathRequest.callback(path, success);
            _isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector2 pathStart;
            public Vector2 pathEnd;
            public Action<Vector2[], bool> callback;

            public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
            {
                this.pathStart = pathStart;
                this.pathEnd = pathEnd;
                this.callback = callback;
            }
        }

    }
}
