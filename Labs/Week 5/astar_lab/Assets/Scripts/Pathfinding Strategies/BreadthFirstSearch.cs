using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : PathfindingStrategy
{
    public override void FindPath(Node start, Node target, Grid grid)
    {
        Queue<Node> fronteir = new Queue<Node>();
        HashSet<Node> explored = new HashSet<Node>();

        fronteir.Enqueue(start);

        while (fronteir.Count > 0)
        {
            Node currentNode = fronteir.Dequeue();
            explored.Add(currentNode);

            if (currentNode == target)
            {
                base.RetracePath(start, target, grid);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || explored.Contains(neighbour))
                    continue;

                neighbour.parent = currentNode;
                if (!fronteir.Contains(neighbour))
                    fronteir.Enqueue(neighbour);
            }
        }
    }

    protected override bool IsRealtime()
    {
        return true;
    }
}
