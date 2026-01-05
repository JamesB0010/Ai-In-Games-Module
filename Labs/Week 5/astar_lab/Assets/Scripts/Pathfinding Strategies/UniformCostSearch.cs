using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniformCostSearch : PathfindingStrategy
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
                //check for smallest cost g cost
                if (frontier[i].gCost < currentNode.gCost)
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

                neighbour.parent = currentNode;
                if (!frontier.Contains(neighbour))
                    frontier.Add(neighbour);
            }
        }
    }

    protected override bool IsRealtime()
    {
        return true;
    }
}
