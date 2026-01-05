using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public bool walkable;
    public Vector3 worldPosition;

    public float gCost;
    public float hCost;

    public float fCost => gCost + hCost;


    public Node parent;


    protected Node(bool walkable, Vector3 worldPos)
    {
        this.walkable = walkable;
        this.worldPosition = worldPos;
    }
}
