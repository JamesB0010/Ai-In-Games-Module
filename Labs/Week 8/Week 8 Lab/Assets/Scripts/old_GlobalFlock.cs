using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class old_GlobalFlock : BaseGlobalFlock
{
    public static old_GlobalFlock instance = null;
    
    [SerializeField] private Transform boidPrefab;
    public static Transform[] Birds => old_GlobalFlock.instance.birds;


    private old_FlockEntity[] flockEntities;

    public static old_FlockEntity[] FlockEntities => instance.flockEntities;
    
    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }

        throw (new Exception("instance of global flock already exists!"));
    }
    public void Start()
    {
        this.birds = new Transform[this.numBirds];
        Vector3 playerPos = player.transform.position;
        float randomXPos = Random.Range(playerPos.x - airSize, playerPos.x + airSize);
        float randomYPos = Random.Range(playerPos.y + 10f, playerPos.y + airSize);
        float randomZPos = Random.Range(playerPos.z - airSize, playerPos.z + airSize);
        old_GlobalFlock.instance.goalPos = new Vector3(randomXPos, randomYPos, randomZPos);


        this.SpawnBirds();
    }

    private void SpawnBirds()
    {
        GameObject birdContainer = new GameObject("Bird Container");
        birdContainer.transform.position = Vector3.zero;
        
        Vector3 playerPos = player.transform.position;
        for (int i = 0; i < old_GlobalFlock.instance.numBirds; ++i)
        {
            float randomXPos = Random.Range(playerPos.x - airSize, playerPos.x + airSize);
            float randomYPos = Random.Range(playerPos.y + 10f, playerPos.y + airSize);
            float randomZPos = Random.Range(playerPos.z - airSize, playerPos.z + airSize);
            Vector3 spawnPos = new Vector3(randomXPos, randomYPos, randomZPos);

            old_GlobalFlock.Birds[i] = base.SpawnBird(this.boidPrefab, spawnPos,
                Quaternion.LookRotation(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))),
                birdContainer.transform);

        }

        flockEntities = Array.ConvertAll(old_GlobalFlock.Birds, b => b.GetComponent<old_FlockEntity>());

        foreach (old_FlockEntity oldFlockEntity in flockEntities)
        {
            oldFlockEntity.Player = base.player;
        }
    }

    public void Update()
    {
        if (Random.Range(0, 100) < 1) //1% chance to update goal pos
        {
            Vector3 playerPos = player.transform.position;
            float randomXPos = Random.Range(playerPos.x - airSize, playerPos.x + airSize);
            float randomYPos = Random.Range(playerPos.y + 10f, playerPos.y + airSize);
            float randomZPos = Random.Range(playerPos.z - airSize, playerPos.z + airSize);
            old_GlobalFlock.instance.goalPos = new Vector3(randomXPos, randomYPos, randomZPos);
        }
    }
}
