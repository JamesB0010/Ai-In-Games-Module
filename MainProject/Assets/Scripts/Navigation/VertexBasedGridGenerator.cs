using System;
using System.Collections;
using System.Collections.Generic;
using Navigation;
using UnityEngine;



public class VertexBasedGridGenerator : GridGenerator
{
    [SerializeField] private Transform CentroidVisualiserPrefab;
    [SerializeField] private bool spawnVisualizers;

    [SerializeField] private int terrainGenStepSize;
    
    
    public override NavigationNodeCollection GenerateGrid(GameObject terrain)
    {
        NodeLattice nodeLattice = new NodeLattice();
        GameObject centroidsParent;
        centroidsParent = new GameObject("Centroids Parent");
        centroidsParent.transform.parent = terrain.transform;
        CentroidConnector centroidConnector = new CentroidConnector();

        int[] triangleIndicies;
        Vector3[] meshVerticies;
        if (terrain.TryGetComponent(out MeshFilter meshFilter))
        {
            triangleIndicies = meshFilter.mesh.triangles;
            meshVerticies = meshFilter.mesh.vertices;
        }
        else
        {
            if (terrain.TryGetComponent(out Terrain terrainComponent))
            {
                var data = this.GetTriangleIndiciesAndMeshVerts(terrainComponent, this.terrainGenStepSize);
                //get from terrain component
                triangleIndicies = data.Key;
                meshVerticies = data.Value;
            }
            else
            {
                return null;
            }
        }


        for (int i = 0; i < triangleIndicies.Length; i += 3)
        {
            Vector3 vertex0 = meshVerticies[triangleIndicies[i]];
            Vector3 vertex1 = meshVerticies[triangleIndicies[i + 1]];
            Vector3 vertex2 = meshVerticies[triangleIndicies[i + 2]];

            centroidConnector.AddVertIndexToIndexToTriangles(triangleIndicies[i], i / 3);
            centroidConnector.AddVertIndexToIndexToTriangles(triangleIndicies[i + 1], i / 3);
            centroidConnector.AddVertIndexToIndexToTriangles(triangleIndicies[i + 2],i / 3);
            
            vertex0 = terrain.transform.localToWorldMatrix.MultiplyPoint(vertex0);
            vertex1 = terrain.transform.localToWorldMatrix.MultiplyPoint(vertex1);
            vertex2 = terrain.transform.localToWorldMatrix.MultiplyPoint(vertex2);
            Vector3 centroidLocation = vertex0 + vertex1 + vertex2;
            centroidLocation /= 3;

            if(this.spawnVisualizers)
                GameObject.Instantiate(this.CentroidVisualiserPrefab, centroidLocation, Quaternion.identity).parent =
                centroidsParent.transform;
            
            
            bool walkable = !(Physics.CheckSphere(centroidLocation, nodeRadius, unwalkableMask));
            NavMeshNode node = new NavMeshNode(walkable, centroidLocation);
            centroidConnector.AddNode(i / 3, node);
            nodeLattice.TrackNode(node);
        }

        centroidConnector.LinkNodesTogether();
        
        return nodeLattice;
    }

   private KeyValuePair<int[], Vector3[]> GetTriangleIndiciesAndMeshVerts(Terrain terrain, int stepSize)
   {
       // Terrains don't actually have a list of vertices; this information has to be generated.
       TerrainData terrainData = terrain.terrainData;
       Vector3 terrainPosition = terrain.transform.position;
       Vector3 terrainSize = terrainData.size;
   
       int resolution = terrainData.heightmapResolution;
   
       List<Vector3> vertexPositions = new();
       List<int> vertexIndices = new();
   
       int vertexIndex = 0;
   
       for (int y = 0; y < resolution; y += stepSize)
       {
           for (int x = 0; x < resolution; x += stepSize)
           {
               float height = terrainData.GetHeight(y, x);
   
               vertexPositions.Add(this.GridPointToWorld(new Vector2(x, y), resolution, height, terrainPosition,
                   terrainSize));
   
               // Create triangles if not on the edge
               if (x < resolution - stepSize && y < resolution - stepSize)
               {
                   int topLeft = vertexIndex;
                   int topRight = vertexIndex + 1;
                   int bottomLeft = vertexIndex + (resolution / stepSize);
                   int bottomRight = bottomLeft + 1;
   
                   // First triangle
                   vertexIndices.Add(topLeft);
                   vertexIndices.Add(bottomLeft);
                   vertexIndices.Add(topRight);
   
                   // Second triangle
                   vertexIndices.Add(topRight);
                   vertexIndices.Add(bottomLeft);
                   vertexIndices.Add(bottomRight);
               }
   
               vertexIndex++;
           }
       }
   
       return new KeyValuePair<int[], Vector3[]>(vertexIndices.ToArray(), vertexPositions.ToArray());
   }


    private Vector3 GridPointToWorld(Vector2 gridCoordinate, int resolution, float height, Vector3 terrainPosition, Vector3 terrainSize)
    {
        float worldX = gridCoordinate.x / (resolution - 1) * terrainSize.x + terrainPosition.x;
        float worldY = height + terrainPosition.y;
        float worldZ = gridCoordinate.y / (resolution - 1) * terrainSize.z + terrainPosition.z;

        return new Vector3(worldX, worldY, worldZ);
    }
}
