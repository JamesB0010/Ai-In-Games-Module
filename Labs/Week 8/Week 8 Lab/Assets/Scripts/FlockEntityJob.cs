using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;


    [BurstCompile]
    public struct FlockEntityJob : IJobParallelForTransform
    {
        // Delta time must be copied to the job since jobs generally don't have a concept of a frame.
        // The main thread waits for the job same frame or next frame, but the job should do work deterministically
        // independent on when the job happens to run on the worker threads.
        public float deltaTime;
        
        [ReadOnly]
        public NativeArray<float> playerPosition;

        [ReadOnly]
        public NativeArray<Vector3> birdPositions;

        public NativeArray<float> birdSpeeds;

        [ReadOnly] public NativeArray<float> birdSpeedsReadOnly;

        public bool turning;

        public Unity.Mathematics.Random random;

        public float rotSpeed;

        public float speed;
        
        public float neighbourDistance;

        public float3 goalPos;

        // The code actually running on the job
        public void Execute(int index, TransformAccess transform)
        {
            this.random = new Unity.Mathematics.Random(this.random.NextUInt() + (uint)index);
            
            Vector3 playerPositionVec =
                new Vector3(this.playerPosition[0], this.playerPosition[1], this.playerPosition[2]);

            if (Vector3.Distance(transform.position, playerPositionVec) >= GlobalFlock.AirSize)
            {
                this.turning = true;
            }else if (transform.position.y <= 10.0f)
            {
                this.turning = true;
            }
            else
            {
                this.turning = false;
            }

            if (this.turning)
            {
                Vector3 direction = playerPositionVec + new Vector3(0, this.random.NextFloat(10f, GlobalFlock.AirSize), 0) - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                                this.rotSpeed * this.deltaTime);
                this.speed = this.random.NextFloat(0.5f, 3f);
            }

            if (this.random.NextFloat(0, 5) < 1) //20% of the time
            {
                //good stuff happens!
                Vector3 vCentre = transform.position;
                Vector3 vAvoid = transform.position;


                float gSpeed = 0.5f;
                Vector3 goalPos = this.goalPos;

                float dist;
                int groupSize = 0;

                for (int i = 0; i < this.birdPositions.Length; ++i)
                {
                    if (birdPositions[i] == transform.position)
                        return;
                    dist = Vector3.Distance(birdPositions[i], transform.position);
                    if (dist <= this.neighbourDistance)
                    {
                        vCentre += birdPositions[i];
                        groupSize++;
                        if (dist < 6f)
                        {
                            vAvoid = vAvoid + (transform.position - birdPositions[i]); //separation
                        }

                        gSpeed = gSpeed + birdSpeedsReadOnly[i];
                    }
                }

                if (groupSize >= 0)
                {
                    vCentre = vCentre / groupSize + (goalPos - transform.position); //cohesion
                    this.speed = (gSpeed / groupSize) + Random.Range(-0.1f, 0.1f);
                    if (this.speed > 5f)
                    {
                        this.speed = Random.Range(0.5f, 3f);
                    }

                    Vector3 direction = (vCentre + vAvoid).normalized; //alignment
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(direction),
                        rotSpeed * this.deltaTime);
                }
            }


            this.birdSpeeds[index] = this.speed;
            transform.position += new Vector3(0, 0, this.deltaTime * this.speed);
        }
    }
