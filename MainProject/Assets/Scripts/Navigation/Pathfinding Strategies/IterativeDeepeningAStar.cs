using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Navigation;
using UnityEngine;


public class IterativeDeepeningAStar : PathfindingStrategy
{
private class IterativeDeepeningPath: List<Node>
{
   public bool foundGoal = false;

   public IterativeDeepeningPath(List<Node> path)
   {
       this.AddRange(path);
       foundGoal = true;
   }

   public IterativeDeepeningPath(){}
}

    public override List<Node> FindPath(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer)
    {
        IterativeDeepeningPath path = new IterativeDeepeningPath();
        
        if (!target.walkable)
        {
            return path; // Exit early
        }

        int depthLimit = 0;
        while (true)
        {
            List<Node> frontier = new();
            HashSet<Node> visited = new();
            int currentDepth = 0; // Reset depth here

            bool targetFound = Search(start, target, boundsGeneratedGrid, gridPathRenderer, currentDepth, depthLimit, frontier, visited).foundGoal;
            if (targetFound)
                return path;

            depthLimit++;
        }
    }

    private IterativeDeepeningPath Search(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer, int currentDepth, int depthLimit, List<Node> frontier, HashSet<Node> visited)
    {
        frontier.Add(start);

        while (frontier.Count > 0)
        {
            if (currentDepth >= depthLimit)
            {
                frontier.Clear(); // Clear frontier to avoid infinite loop
                return new IterativeDeepeningPath();
            }

            Node currentGridNode = frontier[0];
            for (int i = 1; i < frontier.Count; i++)
            {
                if (frontier[i].fCost < currentGridNode.fCost || (frontier[i].fCost == currentGridNode.fCost && frontier[i].hCost < currentGridNode.hCost))
                {
                    currentGridNode = frontier[i];
                }
            }

            frontier.Remove(currentGridNode);
            visited.Add(currentGridNode);

            if (currentGridNode == target)
            {
                var path = new IterativeDeepeningPath(base.RetracePath(start, target, gridPathRenderer));
                return path;
            }

            foreach (Node neighbour in boundsGeneratedGrid.GetNeighbours(currentGridNode))
            {
                if (!neighbour.walkable || visited.Contains(neighbour))
                    continue;

                float newMovementCostToNeighbour = currentGridNode.gCost + base.GetDistance(currentGridNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !frontier.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = base.GetDistance(neighbour, target);
                    neighbour.parent = currentGridNode;

                    if (!frontier.Contains(neighbour))
                        frontier.Add(neighbour);
                }
            }
            currentDepth++; // Increment currentDepth here
        }
        return new IterativeDeepeningPath();
    }
}


