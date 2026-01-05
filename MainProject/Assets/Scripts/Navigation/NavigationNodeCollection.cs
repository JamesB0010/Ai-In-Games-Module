using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NavigationNodeCollection
{
    public abstract Node NodeFromWorldPoint(Vector3 worldPosition);

    public abstract List<Node> GetNeighbours(Node node);


    public List<Node> GetNodes()
    {
        return nodes;
    }

    public void TrackNode(Node node)
    {
        this.nodes.Add(node);
    }

    protected List<Node> nodes = new List<Node>();
}
