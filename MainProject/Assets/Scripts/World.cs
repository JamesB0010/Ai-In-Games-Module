using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World
{
    private static readonly World instance = new World();
    private static WorldStates world;

    private Queue<GameObject> patients;

    private Queue<GameObject> cubicles;

    static World()
    {
        World.world = new WorldStates();
        instance.DiscoverCubicles();
    }
    private World()
    {
        this.patients = new Queue<GameObject>();
        this.cubicles = new Queue<GameObject>();
    }

    private void DiscoverCubicles()
    {
        GameObject[] cubs = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach (GameObject cubicle in cubs)
        {
            this.cubicles.Enqueue(cubicle);
            World.world.ModifyState("freeCubicle", 1);
        }
    }

    public void AddPatient(GameObject patient)
    {
        this.patients.Enqueue(patient);
    }

    public GameObject RemovePatient()
    {
        bool queueEmpty = patients.Count == 0;
        return  queueEmpty ? null : patients.Dequeue();
    }

    public GameObject RemoveCubicle()
    {
        bool queueEmpty = this.cubicles.Count == 0;
        return queueEmpty ? null : cubicles.Dequeue();
    }

    public static World Instance => World.instance;

    public WorldStates GetWorld()
    {
        return world;
    }
}
