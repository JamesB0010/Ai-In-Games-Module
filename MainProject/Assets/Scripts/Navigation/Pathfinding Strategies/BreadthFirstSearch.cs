using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : PathfindingStrategy
{
    public override List<Node> FindPath(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer)
    {
        Queue<Node> fronteir = new Queue<Node>();
        HashSet<Node> explored = new HashSet<Node>();

        fronteir.Enqueue(start);

        while (fronteir.Count > 0)
        {
            Node currentGridNode = fronteir.Dequeue();
            explored.Add(currentGridNode);

            if (currentGridNode == target)
            {
                return base.RetracePath(start, target, gridPathRenderer);
            }

            foreach (Node neighbour in boundsGeneratedGrid.GetNeighbours(currentGridNode))
            {
                if (!neighbour.walkable || explored.Contains(neighbour))
                    continue;

                neighbour.parent = currentGridNode;
                if (!fronteir.Contains(neighbour))
                    fronteir.Enqueue(neighbour);
            }
        }

        return null;
    }
}
