using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlockMaster : MonoBehaviour
{
    [Serializable]
    public enum FlockThreadMode : byte
    {
        SingleThread,
        Multithreaded
    }

    [SerializeField] private FlockThreadMode flockThreadMode;

    [SerializeField] private singleThreadGlobalFlock singleThreadGlobalFlock;

    [SerializeField] private MultiThreadedGlobalFlock multiThreadedGlobalFlock;

    [SerializeField] private Transform player;

    private void Awake()
    {
        if (flockThreadMode == FlockThreadMode.SingleThread)
        {
            this.multiThreadedGlobalFlock = null;
            singleThreadGlobalFlock.Awake();
            singleThreadGlobalFlock.player = this.player;
        }
        else
        {
            this.singleThreadGlobalFlock = null;
            this.multiThreadedGlobalFlock.player = this.player;
        }
    }

    private void Start()
    {
        if (flockThreadMode == FlockThreadMode.SingleThread)
        {
            this.singleThreadGlobalFlock.SpawnBird = Instantiate;
            singleThreadGlobalFlock.Start();
        }
        else
        {
            this.multiThreadedGlobalFlock.SpawnBird = Instantiate;
            this.multiThreadedGlobalFlock.Start();
        }
    }


    private void Update()
    {
        if (flockThreadMode == FlockThreadMode.SingleThread)
        {
            this.singleThreadGlobalFlock.Update();
        }
        else
        {
            this.multiThreadedGlobalFlock.Update();
        }
    }


    private void OnDestroy()
    {
        singleThreadGlobalFlock.instance = null;

        if (this.flockThreadMode == FlockThreadMode.Multithreaded)
            multiThreadedGlobalFlock?.OnDestroy();
    }
}