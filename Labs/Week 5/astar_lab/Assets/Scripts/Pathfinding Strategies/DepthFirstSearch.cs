using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : PathfindingStrategy
{

    public override void FindPath(Node start, Node target, Grid grid)
    {
        Stack<Node> fronteir = new Stack<Node>();
        HashSet<Node> explored = new HashSet<Node>();

        fronteir.Push(start);

        while (fronteir.Count > 0)
        {
            Node currentNode = fronteir.Pop();
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
                if (!explored.Contains(neighbour))
                    fronteir.Push(neighbour);
            }
        }

    }
    protected override bool IsRealtime()
    {
        return true;
    }
}
