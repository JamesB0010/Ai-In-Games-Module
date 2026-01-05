using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshNode : Node
{
    public NavMeshNode(bool walkable, Vector3 worldPos) : base(walkable, worldPos)
    {
    }

    private List<NavMeshNode> neighbours = new List<NavMeshNode>();
    public List<NavMeshNode> Neighbours => this.neighbours;

    public bool HasNeighbour(NavMeshNode node)
    {
        return this.neighbours.Contains(node);
    }

    public void AddNeighbour(NavMeshNode node)
    {
        this.neighbours.Add(node);
    }
}
