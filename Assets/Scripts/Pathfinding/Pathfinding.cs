using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SG.Path
{
    public class Pathfinding : MonoBehaviour
    {
        PathRequestManager _pathRequestManager;
        Grid _grid;

        void Awake()
        {
            _pathRequestManager = GetComponent<PathRequestManager>();
            _grid = GetComponent<Grid>();
        }



        public void StartFindPath(Vector2 startPos, Vector2 targetPos)
        {
            StartCoroutine(FindPath(startPos, targetPos));
        }

        IEnumerator FindPath(Vector2 startPos, Vector2 targetPos)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Vector2[] waypoints = new Vector2[0];
            bool pathSuccess = false;

            Node startNode = _grid.NodeFromWorldPoint(startPos);
            Node targetNode = _grid.NodeFromWorldPoint(targetPos);

            if (targetNode.walkable)
            {
                Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);
                while (openSet.Count > 0)
                {
                    Node node = openSet.RemoveFirst();
                    closedSet.Add(node);

                    if (node == targetNode)
                    {
                        sw.Stop();
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in _grid.GetNeighbours(node))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                        if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = node;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
            yield return null;
            if (pathSuccess)
            {
                waypoints = RetracePath(startNode, targetNode);
                _pathRequestManager.FinishedProcessingPath(waypoints, pathSuccess);
            }
            //if (!targetNode.walkable)
            //{
               // _pathRequestManager.FinishedProcessingPath(new Vector2[0], true);
            //}
        }

        Vector2[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            if (currentNode == startNode)
                path.Add(currentNode);
            Vector2[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        private Vector2[] SimplifyPath(List<Node> path)
        {
            List<Vector2> waypoints = new List<Vector2>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);

                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i -1].worldPosition);
                }
                directionOld = directionNew;
            }
            return waypoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
