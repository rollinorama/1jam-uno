using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG.Path
{
    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector2 worldPosition;
        public int gridX;
        public int gridY;
        public int gCost;
        public int hCost;
        public Node parent;
        private int _heapIndex;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int HeapIndex
        {
            get
            {
                return _heapIndex;
            }
            set
            {
                _heapIndex = value;
            }
        }

        public int CompareTo(Node nodeCompare)
        {
            int compare = fCost.CompareTo(nodeCompare.fCost);
            if(compare == 0)
            {
                compare = hCost.CompareTo(nodeCompare.hCost);
            }
            return -compare;
        }
    }
}
