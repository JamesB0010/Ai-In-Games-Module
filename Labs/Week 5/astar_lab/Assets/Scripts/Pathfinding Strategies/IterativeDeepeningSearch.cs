using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IterativeDeepeningSearch : PathfindingStrategy
{
    public override void FindPath(Node start, Node target, Grid grid)
    {
        int depthLimit = 0;
        HashSet<Node> explored = new HashSet<Node> { start };

        while (!this.VisitNode(start, target, grid, ref explored, 0, depthLimit))
        {
            depthLimit++;
            explored = new HashSet<Node>(){start};
        }
        base.RetracePath(start, target, grid);
    }

    private bool VisitNode(Node node, Node nodeToFind, Grid grid, ref HashSet<Node> explored, int currentDepth, int depthLimit)
    {
        if (currentDepth > depthLimit)
            return false;

        explored.Add(node);

        if (node == nodeToFind)
            return true;

        foreach (Node neighbour in grid.GetNeighbours(node))
        {
            if (!neighbour.walkable || explored.Contains(neighbour))
                continue;

            neighbour.parent = node;
            if (!explored.Contains(neighbour))
                if (this.VisitNode(neighbour, nodeToFind, grid, ref explored, currentDepth + 1, depthLimit))
                    return true;
        }

        return false;
    }
    protected override bool IsRealtime()
    {
        return true;
    }
}
