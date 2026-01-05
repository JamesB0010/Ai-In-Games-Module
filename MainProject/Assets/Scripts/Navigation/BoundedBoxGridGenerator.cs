using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoundedBoxGridGenerator : GridGenerator
{

    public Vector2 gridWorldSize;
    private float nodeDiameter;
    public float NodeDiameter => this.nodeDiameter;

    int gridSizeX, gridSizeY;

    public override NavigationNodeCollection GenerateGrid(GameObject terrain)
    {
        if (terrain.TryGetComponent(out Terrain t))
            return this.GenerateGridForHeightmapTerrain(t);
        else
            return this.GenerateGridForMeshFilterTerrain(terrain);
    }

    private Navigation.BoundsGeneratedGrid GenerateGridForMeshFilterTerrain(GameObject terrain)
    {
        Vector3 worldBottomLeft = terrain.transform.position - Vector3.right * gridWorldSize.x / 2 -
                                  Vector3.forward * gridWorldSize.y / 2;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);


        Navigation.BoundsGeneratedGrid boundsGeneratedGrid = new(gridSizeX, gridSizeY, this.gridWorldSize);


        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                     Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                GridNode node = new GridNode(walkable, worldPoint, x, y);
                boundsGeneratedGrid[x, y] = node;
                boundsGeneratedGrid.TrackNode(node);
            }
        }

        return boundsGeneratedGrid;
    }

    private Navigation.BoundsGeneratedGrid GenerateGridForHeightmapTerrain(Terrain terrain)
{
    Vector3 worldBottomLeft = terrain.transform.position;

    nodeDiameter = nodeRadius * 2;
    gridSizeX = Mathf.RoundToInt(terrain.terrainData.size.x / nodeDiameter);
    gridSizeY = Mathf.RoundToInt(terrain.terrainData.size.z / nodeDiameter);

    Navigation.BoundsGeneratedGrid boundsGeneratedGrid = new(gridSizeX, gridSizeY, new Vector2(terrain.terrainData.size.x, terrain.terrainData.size.z));

    // Precise scaling from terrain size to heightmap resolution
    float terrainToGridScaleX = (float)terrain.terrainData.heightmapResolution / terrain.terrainData.size.x;
    float terrainToGridScaleY = (float)terrain.terrainData.heightmapResolution / terrain.terrainData.size.z;

    for (int x = 0; x < gridSizeX; x++)
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            int terrainX = Mathf.RoundToInt((x * nodeDiameter + nodeRadius) * terrainToGridScaleX);
            int terrainY = Mathf.RoundToInt((y * nodeDiameter + nodeRadius) * terrainToGridScaleY);

            terrainX = Mathf.Clamp(terrainX, 0, terrain.terrainData.heightmapResolution - 1);
            terrainY = Mathf.Clamp(terrainY, 0, terrain.terrainData.heightmapResolution - 1);

            
            float height = terrain.terrainData.GetHeight(terrainX, terrainY);
            
            
            Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                 Vector3.forward * (y * nodeDiameter + nodeRadius) +
                                 Vector3.up * height;

            bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
            GridNode node = new GridNode(walkable, worldPoint, x, y);
            boundsGeneratedGrid[x, y] = node;
            boundsGeneratedGrid.TrackNode(node);
        }
    }

    return boundsGeneratedGrid;
}
}