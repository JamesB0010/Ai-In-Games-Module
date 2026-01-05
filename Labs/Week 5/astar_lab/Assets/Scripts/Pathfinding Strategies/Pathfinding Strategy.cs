using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathfindingStrategy : ScriptableObject
{
    protected abstract bool IsRealtime();
    public bool Realtime
    {
        get
        {
            return IsRealtime();
        }
    }
    public abstract void FindPath(Node start, Node target, Grid grid);

    protected int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * distanceX - distanceY;
        else
            return 14 * distanceX + 10 * distanceY - distanceX;
    }

    public void RetracePath(Node startNode, Node endNode, Grid grid)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);

            currentNode = currentNode.parent;
        }


        path.Reverse();

        grid.path = path;

    }
}
