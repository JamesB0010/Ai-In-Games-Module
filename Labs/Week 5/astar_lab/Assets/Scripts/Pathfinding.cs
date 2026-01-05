using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private PathfindingStrategy pathfindingStrategy;

    //this is useful for slow algorithms that should only be ran once
    bool pathfindPeformed = false;

    Grid grid;

    [SerializeField]
    Transform seeker;

    [SerializeField]
    Transform target;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (pathfindingStrategy.Realtime == false)
        {
            if (pathfindPeformed == false)
            {
                this.pathfindPeformed = true;
                this.FindPath(seeker.position, target.position);
            }
            return;
        }


        this.FindPath(seeker.position, target.position);
    }


    void FindPath(Vector3 start, Vector3 target)
    {
        Node startNode = this.grid.NodeFromWorldPoint(start);
        Node endNode = this.grid.NodeFromWorldPoint(target);

        this.pathfindingStrategy.FindPath(startNode, endNode, grid);
    }
}
