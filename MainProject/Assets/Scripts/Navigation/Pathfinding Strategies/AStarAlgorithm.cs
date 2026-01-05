using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : PathfindingStrategy
{
    public override List<Node> FindPath(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer)
    {
        List<Node> frontier = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        frontier.Add(start);

        while (frontier.Count > 0)
        {
            Node currentGridNode = frontier[0];
            for (int i = 1; i < frontier.Count; i++)
            {
                if (frontier[i].fCost < currentGridNode.fCost || frontier[i].fCost == currentGridNode.fCost && frontier[i].hCost < currentGridNode.hCost)
                {
                    currentGridNode = frontier[i];
                }
            }

            frontier.Remove(currentGridNode);
            visited.Add(currentGridNode);

            if (currentGridNode == target)
            {
                return base.RetracePath(start, target, gridPathRenderer);
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
        }

        return null;
    }
}
