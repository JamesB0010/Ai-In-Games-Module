using System.Collections;
using System.Collections.Generic;
using Navigation;
using UnityEngine;

public class IterativeDeepeningSearch : PathfindingStrategy
{
    public override List<Node> FindPath(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer)
    {
        int depthLimit = 0;
        HashSet<Node> explored = new HashSet<Node> { start };

        while (!this.VisitNode(start, target, boundsGeneratedGrid, ref explored, 0, depthLimit))
        {
            depthLimit++;
            explored = new HashSet<Node>() { start };
        }
        return base.RetracePath(start, target, gridPathRenderer);
    }

    private bool VisitNode(Node gridNode, Node gridNodeToFind, NavigationNodeCollection boundsGeneratedGrid, ref HashSet<Node> explored, int currentDepth, int depthLimit)
    {
        if (currentDepth > depthLimit)
            return false;

        explored.Add(gridNode);

        if (gridNode == gridNodeToFind)
            return true;

        foreach (Node neighbour in boundsGeneratedGrid.GetNeighbours(gridNode))
        {
            if (!neighbour.walkable || explored.Contains(neighbour))
                continue;

            neighbour.parent = gridNode;
            if (!explored.Contains(neighbour))
                if (this.VisitNode(neighbour, gridNodeToFind, boundsGeneratedGrid, ref explored, currentDepth + 1, depthLimit))
                    return true;
        }

        return false;
    }
}
