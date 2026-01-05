using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathfindingStrategy : ScriptableObject
{
    public abstract List<Node> FindPath(Node start, Node target, NavigationNodeCollection boundsGeneratedGrid, GridPathRenderer gridPathRenderer);

    protected float GetDistance(Node nodeA, Node nodeB)
    {
        float distanceX;
        float distanceY;

        //if the two nodes passed in are grid nodes then calculate using grid index
        if (nodeA is GridNode gridNodeA && nodeB is GridNode gridNodeB)
        {
            distanceX = Mathf.Abs(gridNodeA.gridX - gridNodeB.gridX);
            distanceY = Mathf.Abs(gridNodeA.gridY - gridNodeB.gridY);
            //euclidean distance
            return distanceX * distanceX + distanceY * distanceY;
        }
        
        
        //Nodes must be navmesh nodes so calculate via world position
        
        distanceX = Mathf.Abs(nodeA.worldPosition.x - nodeB.worldPosition.x);
        distanceY = Mathf.Abs(nodeA.worldPosition.y - nodeB.worldPosition.y);

        //euclidean distance
        return distanceX * distanceX + distanceY * distanceY;
    }

    public List<Node> RetracePath(Node startGridNode, Node endGridNode, GridPathRenderer gridPathRenderer)
    {
        List<Node> path = new List<Node>();
        Node currentGridNode = endGridNode;

        while (currentGridNode != startGridNode)
        {
            path.Add(currentGridNode);

            currentGridNode = currentGridNode.parent;
        }


        path.Reverse();

        return path;
    }
}
