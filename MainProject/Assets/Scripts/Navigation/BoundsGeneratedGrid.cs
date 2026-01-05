using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigation
{
    public class BoundsGeneratedGrid : NavigationNodeCollection
    {
        private GridNode[,] grid;

        public GridNode[,] GridProperty
        {
            set => this.grid = value;
        }

        public BoundsGeneratedGrid(int width, int height, Vector2 gridWorldSize)
        {
            this.grid = new GridNode[width, height];
            this.gridSizeX = width;
            this.gridSizeY = height;
            this.gridWorldSize = gridWorldSize;
        }

        public GridNode this[int row, int column]
        {
            get => this.grid[row, column];
            set => this.grid[row, column] = value;
        }

        private float gridSizeX, gridSizeY;
        private Vector2 gridWorldSize;

        

        public override Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            /*float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];*/

            Node closestNode = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < this.nodes.Count; i++)
            {
                float dist = Vector3.Distance(this.nodes[i].worldPosition, worldPosition);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestNode = this.nodes[i];
                }
            }

            return closestNode;
        }

        public override List<Node> GetNeighbours(Node node)
        {
            GridNode gridNode = node as GridNode;
            if (gridNode == null)
                throw new ArgumentException("Argument was not of the concrete type GridNode");
            
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = gridNode.gridX + x;
                    int checkY = gridNode.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        
    }
}
