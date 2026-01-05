using System;
using System.Collections;
using System.Collections.Generic;
using Navigation;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private static Pathfinding instance = null;

    public static event Action pathfindingStratChanged;
    
    [Header("Configurables")]
    [SerializeField]
    private PathfindingStrategy pathfindingStrategy;

    private PathfindingStrategy previousPathfindingStrategy;

    [Space(2)]
    [SerializeField] GridGenerator gridGenerator;

    public GridGenerator GridGenerator => this.gridGenerator;
    private GridGenerator previousGridGenerator = null;

    private MeshFilter terrainMeshFilter = null;
    public MeshFilter TerrainMeshFilter
    {
        get
        {
            if (terrainMeshFilter == null)
                this.terrainMeshFilter = this.terrain.GetComponent<MeshFilter>();

            return this.terrainMeshFilter;
        }
    }

    private NavigationNodeCollection nodeCollection;
    public NavigationNodeCollection NodeCollection => this.nodeCollection;

    [Space(8)]
    [Header("Dependencies")]

    //this is useful for slow algorithms that should only be ran once
    bool pathfindPeformed = false;
    

    [SerializeField] private GameObject terrain;
    public Transform Terrain => this.terrain.transform;

    private GridPathRenderer gridPathRenderer = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        } 
        
        Destroy(this.gameObject);
    }

    private void Start()
    {
        this.previousPathfindingStrategy = this.pathfindingStrategy;
        nodeCollection = this.gridGenerator.GenerateGrid(this.terrain);

        if (TryGetComponent(out GridPathRenderer renderer))
            this.gridPathRenderer = renderer;

        this.previousGridGenerator = this.gridGenerator;
    }

    private void Update()
    {
        if (this.pathfindingStrategy != previousPathfindingStrategy)
            pathfindingStratChanged?.Invoke();
        
        if (this.gridGenerator != this.previousGridGenerator)
        {
            this.previousGridGenerator = this.gridGenerator;
            this.nodeCollection = this.gridGenerator.GenerateGrid(this.terrain);
        }
    }


    public static List<Node> FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        if (instance == null)
        {
            Debug.LogError("Cannot do pathfinding as no pathfinding object is in the scene");
            return null;
        }
        
        Node startGridNode = instance.nodeCollection.NodeFromWorldPoint(startPosition);
        Node endGridNode = instance.nodeCollection.NodeFromWorldPoint(targetPosition);

        return instance.pathfindingStrategy.FindPath(startGridNode, endGridNode, instance.nodeCollection, instance.gridPathRenderer);
    }
}
