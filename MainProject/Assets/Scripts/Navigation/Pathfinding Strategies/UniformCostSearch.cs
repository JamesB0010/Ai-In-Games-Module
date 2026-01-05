using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniformCostSearch : PathfindingStrategy
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
                //check for smallest cost g cost
                if (frontier[i].gCost < currentGridNode.gCost)
                    currentGridNode = frontier[i];
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

                neighbour.parent = currentGridNode;
                if (!frontier.Contains(neighbour))
                    frontier.Add(neighbour);
            }
        }

        return null;
    }
}
