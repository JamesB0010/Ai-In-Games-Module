using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthLimitedSearch : PathfindingStrategy
{
    [SerializeField] private int depthLimit;

    public override void FindPath(Node start, Node target, Grid grid)
    {
        HashSet<Node> explored = new HashSet<Node>();

        if (this.VisitNode(start, target, start, grid, ref explored, 0))
            base.RetracePath(start, target, grid);
    }

    private bool VisitNode(Node node, Node nodeToFind, Node startNode, Grid grid, ref HashSet<Node> explored, int currentDepth)
    {
        if (currentDepth > this.depthLimit)
            return false;

        explored.Add(node);

        if (node == nodeToFind)
        {
            base.RetracePath(startNode, nodeToFind, grid);
            return true;
        }

        foreach (Node neighbour in grid.GetNeighbours(node))
        {
            if (!neighbour.walkable || explored.Contains(neighbour))
                continue;

            neighbour.parent = node;
            if (!explored.Contains(neighbour))
                if (this.VisitNode(neighbour, nodeToFind, startNode, grid, ref explored, currentDepth + 1))
                    return true;
        }

        return false;
    }
    protected override bool IsRealtime()
    {
        return true;
    }
}
