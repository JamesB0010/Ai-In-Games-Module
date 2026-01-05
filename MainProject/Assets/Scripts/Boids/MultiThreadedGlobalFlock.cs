using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

[Serializable]
public class MultiThreadedGlobalFlock : BaseGlobalFlock
{
    [SerializeField] private Transform boidPrefab;
    private NativeArray<float> birdSpeeds;

    private NativeArray<float> birdSpeedsReadOnly;

    private NativeArray<float> playerPosition;

    private NativeArray<Vector3> birdPositions;

    public Transform[] Birds => this.birds;

    private TransformAccessArray m_accessArray;

    private NativeArray<Vector3> velocity;

    private Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);

    public void Start()
    {
        this.birds = new Transform[this.numBirds];
        Vector3 playerPos = player.transform.position;
        float randomXPos = Random.Range(playerPos.x - airSize, playerPos.x + airSize);
        float randomYPos = Random.Range(playerPos.y + 10f, playerPos.y + airSize);
        float randomZPos = Random.Range(playerPos.z - airSize, playerPos.z + airSize);
        this.goalPos = new Vector3(randomXPos, randomYPos, randomZPos);

        this.SpawnBirds();

        this.birdSpeeds = new NativeArray<float>(this.birds.Length, Allocator.Persistent);
        this.birdSpeedsReadOnly = new NativeArray<float>(this.birds.Length, Allocator.Persistent);
        for (int i = 0; i < this.birdSpeeds.Length; ++i)
        {
            this.birdSpeeds[i] = 1;
            this.birdSpeedsReadOnly[i] = 1;
        }

        this.birdPositions = new NativeArray<Vector3>(this.birds.Length, Allocator.Persistent);

        this.playerPosition = new NativeArray<float>(3, Allocator.Persistent);
        playerPosition[0] = this.player.position.x;
        playerPosition[1] = this.player.position.y;
        playerPosition[2] = this.player.position.z;
        
        
        velocity = new NativeArray<Vector3>(birds.Length, Allocator.Persistent);

        for (int i = 0; i < velocity.Length; ++i)
        {
            velocity[i] = new Vector3(0f, 10f, 0f);
        }

        this.m_accessArray = new TransformAccessArray(this.birds);
    }

    public void OnDestroy()
    {
        this.m_accessArray.Dispose();
        this.velocity.Dispose();
        this.birdSpeeds.Dispose();
        this.playerPosition.Dispose();
        this.birdPositions.Dispose();
        this.birdSpeedsReadOnly.Dispose();
    }

    private void SpawnBirds()
    {
        GameObject birdContainer = new GameObject("Bird Container");
        birdContainer.transform.position = Vector3.zero;
        
        Vector3 playerPos = player.transform.position;
        for (int i = 0; i < this.numBirds; ++i)
        {
            float randomXPos = Random.Range(playerPos.x - airSize, playerPos.x + airSize);
            float randomYPos = Random.Range(playerPos.y + 10f, playerPos.y + airSize);
            float randomZPos = Random.Range(playerPos.z - airSize, playerPos.z + airSize);
            Vector3 spawnPos = new Vector3(randomXPos, randomYPos, randomZPos);

            this.birds[i] = base.SpawnBird(this.boidPrefab, spawnPos,
                Quaternion.LookRotation(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1))),
                birdContainer.transform);
        }
    }

    public void Update()
    {
        playerPosition[0] = this.player.position.x;
        playerPosition[1] = this.player.position.y;
        playerPosition[2] = this.player.position.z;
        
        for (int i = 0; i < birds.Length; ++i)
        {
            birdPositions[i] = birds[i].transform.position;
            birdSpeedsReadOnly[i] = birdSpeeds[i];
        }

        var job = new FlockEntityJob()
        {
            deltaTime = Time.deltaTime,
            playerPosition = playerPosition,
            turning = true,
            random = this.random,
            rotSpeed = 3f,
            speed = 1,
            neighbourDistance = 10,
            birdPositions = birdPositions,
            birdSpeeds = birdSpeeds,
            birdSpeedsReadOnly = birdSpeedsReadOnly,
            goalPos = this.goalPos
        };

        var jobHandle = job.Schedule(this.m_accessArray);
        
        jobHandle.Complete();
        

        if (Random.Range(0, 100) < 1) //1% chance to update goal pos
        {
            Vector3 playerPos = player.transform.position;
            float randomXPos = Random.Range(playerPos.x - airSize, playerPos.x + airSize);
            float randomYPos = Random.Range(playerPos.y + 10f, playerPos.y + airSize);
            float randomZPos = Random.Range(playerPos.z - airSize, playerPos.z + airSize);
            this.goalPos = new Vector3(randomXPos, randomYPos, randomZPos);
        }
    }
}
