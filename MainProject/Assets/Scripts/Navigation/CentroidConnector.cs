using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentroidConnector
{
    private Dictionary<int, List<int>> indexToTriangles = new();

    private Dictionary<int, NavMeshNode> triangleToNodes = new();

    public void AddNode(int triangleIndex, NavMeshNode node)
    {
        this.triangleToNodes.Add(triangleIndex, node);
    }

    public void AddVertIndexToIndexToTriangles(int index, int triangleIndex)
    {
        if (!this.indexToTriangles.ContainsKey(index))
        {
            this.indexToTriangles.Add(index, new List<int>());
        }

        this.indexToTriangles[index].Add(triangleIndex);
    }

    public void LinkNodesTogether()
    {
        foreach (KeyValuePair<int, List<int>> indexToTriangles in this.indexToTriangles)
        {
            List<int> connectedTriangles = indexToTriangles.Value;

            for (int i = 0; i < connectedTriangles.Count; ++i)
            {
                for (int j = 0; j < connectedTriangles.Count; ++j)
                {
                    bool isNotSelf = connectedTriangles[i] != connectedTriangles[j];
                    bool notAlreadyConnected = !this.triangleToNodes[connectedTriangles[i]].HasNeighbour(this.triangleToNodes[connectedTriangles[j]]);
                    if (isNotSelf && notAlreadyConnected)
                    {
                        this.triangleToNodes[connectedTriangles[i]]
                            .AddNeighbour(this.triangleToNodes[connectedTriangles[j]]);
                    }
                }
            }
        }
    }
}
