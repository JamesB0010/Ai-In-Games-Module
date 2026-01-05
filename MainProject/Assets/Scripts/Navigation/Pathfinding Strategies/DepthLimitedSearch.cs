using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DepthLimitedSearch : PathfindingStrategy
{
    [SerializeField] private int depthLimit;

    public override List<Node> FindPath(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer)
    {
        HashSet<Node> explored = new HashSet<Node>();

        if (this.VisitNode(start, target, start, boundsGeneratedGrid, ref explored, 0))
            return base.RetracePath(start, target, gridPathRenderer);

        return null;
    }

    private bool VisitNode(Node gridNode, Node gridNodeToFind, Node startGridNode, NavigationNodeCollection boundsGeneratedGrid, ref HashSet<Node> explored, int currentDepth)
    {
        if (currentDepth > this.depthLimit)
            return false;

        explored.Add(gridNode);

        if (gridNode == gridNodeToFind)
        {
            return true;
        }

        foreach (Node neighbour in boundsGeneratedGrid.GetNeighbours(gridNode))
        {
            if (!neighbour.walkable || explored.Contains(neighbour))
                continue;

            neighbour.parent = gridNode;
            if (!explored.Contains(neighbour))
                if (this.VisitNode(neighbour, gridNodeToFind, startGridNode, boundsGeneratedGrid, ref explored, currentDepth + 1))
                    return true;
        }

        return false;
    }
}
