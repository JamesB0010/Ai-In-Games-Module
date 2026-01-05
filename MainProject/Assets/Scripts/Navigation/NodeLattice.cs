using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeLattice : NavigationNodeCollection
{
    public override Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        Node closestNode = this.nodes[0];
        float closestDistance = Mathf.Abs((closestNode.worldPosition - worldPosition).sqrMagnitude);
        foreach (Node node in this.nodes)
        {
            float distance = Mathf.Abs((node.worldPosition - worldPosition).sqrMagnitude);

            bool newClosestDistanceFound = distance < closestDistance;
            if (newClosestDistanceFound)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

    public override List<Node> GetNeighbours(Node node)
    {
        NavMeshNode navNode = node as NavMeshNode;
        if (navNode == null)
            throw new ArgumentException("Argument was a node which did not have the concrete type of NavMeshNode");
        
        return new List<Node>(navNode.Neighbours);
    }

}
