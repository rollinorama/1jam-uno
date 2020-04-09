using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG.Path
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] bool _displayGridGizmos;
        [SerializeField] LayerMask _unwalkableMask;
        [SerializeField] Vector2 _gridWorldSize;
        [SerializeField] float _nodeRadius;
        [SerializeField] float _unwalkableBorder = 3f;

        private Node[,] grid;
        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY;
        

        public int MaxSize
        {
            get
            {
                return _gridSizeX * _gridSizeY;
            }
        }

        void Awake()
        {
            _nodeDiameter = _nodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
            CreateGrid();
        }

        void CreateGrid()
        {
            grid = new Node[_gridSizeX, _gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x / 2 - Vector3.up * _gridWorldSize.y / 2;
            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.up * (y * _nodeDiameter + _nodeRadius);
                    bool walkable = !(Physics2D.OverlapCircle(worldPoint, _nodeRadius * _unwalkableBorder, _unwalkableMask));
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }
        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
            float percentY = (worldPosition.y + _gridWorldSize.y / 2) / _gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            int x = Mathf.RoundToInt((_gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * percentY);
            return grid[x, y];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, _gridWorldSize.y, 1));

            if (grid != null && _displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter - .1f));
                }
            }
        }
    }
}