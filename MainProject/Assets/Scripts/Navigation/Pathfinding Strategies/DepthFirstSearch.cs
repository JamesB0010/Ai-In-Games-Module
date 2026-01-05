using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : PathfindingStrategy
{

    public override List<Node> FindPath(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer)
    {
        Stack<Node> fronteir = new Stack<Node>();
        HashSet<Node> explored = new HashSet<Node>();

        fronteir.Push(start);

        while (fronteir.Count > 0)
        {
            Node currentGridNode = fronteir.Pop();
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
                if (!explored.Contains(neighbour))
                    fronteir.Push(neighbour);
            }
        }

        return null;
    }
}
