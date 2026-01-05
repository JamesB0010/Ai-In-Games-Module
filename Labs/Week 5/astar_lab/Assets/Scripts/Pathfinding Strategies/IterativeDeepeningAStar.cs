using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IterativeDeepeningAStar : PathfindingStrategy
{
    public override void FindPath(Node start, Node target, Grid grid)
    {
        if (!target.walkable)
        {
            return; // Exit early
        }

        int depthLimit = 0;
        while (true)
        {
            List<Node> frontier = new();
            HashSet<Node> visited = new();
            int currentDepth = 0; // Reset depth here

            bool targetFound = Search(start, target, grid, currentDepth, depthLimit, frontier, visited);
            if (targetFound)
                return;

            depthLimit++;
        }
    }

    private bool Search(Node start, Node target, Grid grid, int currentDepth, int depthLimit, List<Node> frontier, HashSet<Node> visited)
    {
        frontier.Add(start);

        while (frontier.Count > 0)
        {
            if (currentDepth >= depthLimit)
            {
                frontier.Clear(); // Clear frontier to avoid infinite loop
                return false;
            }

            Node currentNode = frontier[0];
            for (int i = 1; i < frontier.Count; i++)
            {
                if (frontier[i].fCost < currentNode.fCost || (frontier[i].fCost == currentNode.fCost && frontier[i].hCost < currentNode.hCost))
                {
                    currentNode = frontier[i];
                }
            }

            frontier.Remove(currentNode);
            visited.Add(currentNode);

            if (currentNode == target)
            {
                base.RetracePath(start, target, grid);
                return true;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || visited.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + base.GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !frontier.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = base.GetDistance(neighbour, target);
                    neighbour.parent = currentNode;

                    if (!frontier.Contains(neighbour))
                        frontier.Add(neighbour);
                }
            }
            currentDepth++; // Increment currentDepth here
        }
        return false;
    }

    protected override bool IsRealtime()
    {
        return true;
    }
}


