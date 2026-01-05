using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyBestFirstSearch : PathfindingStrategy
{

    public override void FindPath(Node start, Node target, Grid grid)
    {
        List<Node> frontier = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        frontier.Add(start);

        while (frontier.Count > 0)
        {
            Node currentNode = frontier[0];
            for (int i = 1; i < frontier.Count; i++)
            {
                if (frontier[i].hCost < currentNode.hCost)
                    currentNode = frontier[i];
            }

            frontier.Remove(currentNode);
            visited.Add(currentNode);

            if (currentNode == target)
            {
                base.RetracePath(start, target, grid);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || visited.Contains(neighbour))
                    continue;

                if (!frontier.Contains(neighbour))
                {
                    neighbour.parent = currentNode;
                    frontier.Add(neighbour);
                }
            }

        }

    }
    protected override bool IsRealtime()
    {
        return true;
    }
}
