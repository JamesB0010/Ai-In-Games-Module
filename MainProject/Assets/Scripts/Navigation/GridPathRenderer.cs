using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface PathfollowingAgent
{
    public List<Node> GetPath();
}

[RequireComponent(typeof(Pathfinding))]
public class GridPathRenderer : MonoBehaviour
{
    [SerializeField][HideInInspector] private Pathfinding pathfinding;

    private PathfollowingAgent focusedAgent = null;

    public void SetFocusedAgent(PathfollowingAgent agent)
    {
        this.focusedAgent = agent;
    }

    private void Reset()
    {
        this.pathfinding = GetComponent<Pathfinding>();
    }

    private void OnEnable()
    {
    }

    void OnDrawGizmos()
    {
        if (!this.enabled)
            return;

        switch (this.pathfinding.GridGenerator)
        {
            case BoundedBoxGridGenerator generator:
                this.DrawGizmosForBoundingBoxGrid(generator);
                break;
            case VertexBasedGridGenerator generator:
                this.DrawGizmosForVertexGeneratedMesh(generator);
                break;
            default:
                break;
        }
    }

    private void DrawGizmosForBoundingBoxGrid(BoundedBoxGridGenerator generator)
    {
        DrawCubeAroundTerrain(generator);

        if (this.pathfinding.NodeCollection == null) 
            return;
        
        
        foreach (Node n in pathfinding.NodeCollection.GetNodes())
        {
            Gizmos.color = (n.walkable) ? Color.white : Color.red;
            if (focusedAgent?.GetPath() != null)
                if (focusedAgent.GetPath().Contains(n))
                    Gizmos.color = Color.black;
            Gizmos.DrawCube(n.worldPosition, Vector3.one * (generator.NodeDiameter - .1f));
        }
    }

    private void DrawCubeAroundTerrain(BoundedBoxGridGenerator generator)
    {
        if (pathfinding.Terrain.TryGetComponent(out Terrain terrain))
        {
            float yPos = pathfinding.Terrain.position.y;
            Vector3 cubePos = pathfinding.Terrain.position + terrain.terrainData.size / 2;
            cubePos.y = yPos;

            Gizmos.DrawWireCube(cubePos, terrain.terrainData.size);
        }
        else
            Gizmos.DrawWireCube(pathfinding.Terrain.position,
                new Vector3(generator.gridWorldSize.x, 1, generator.gridWorldSize.y));
    }

    private void DrawGizmosForVertexGeneratedMesh(VertexBasedGridGenerator generator)
    {
        Transform terrainTransform = this.pathfinding.Terrain.transform;
        Vector3 terrainPosition = terrainTransform.position;
        Quaternion terrainRotation = terrainTransform.rotation;
        Vector3 terrainScale = terrainTransform.localScale;
        Gizmos.DrawWireMesh(this.pathfinding.TerrainMeshFilter.sharedMesh, terrainPosition, terrainRotation, terrainScale);
        
        //Draw Centroid connections
        if (!Application.isPlaying)
            return; 
        
        List<Node> nodes = pathfinding.NodeCollection.GetNodes();
        List<NavMeshNode> navNodes = nodes.Select(node => node as NavMeshNode).ToList();

        if (navNodes[0] == null)
            return;

        foreach (NavMeshNode node in navNodes)
        {
            foreach (NavMeshNode neighbour in node.Neighbours)
            {
                bool neightbourIsInPath = false;
                if (focusedAgent.GetPath() != null)
                {
                    if (this.focusedAgent != null)
                    {
                        neightbourIsInPath = this.focusedAgent.GetPath().Contains(neighbour);
                    }
                    else
                        neightbourIsInPath = false;
                }
                
                
                Gizmos.color = neightbourIsInPath ? Color.green : Color.black;
                Gizmos.DrawLine(node.worldPosition, neighbour.worldPosition);
            }
        }
        
        
    }
}
