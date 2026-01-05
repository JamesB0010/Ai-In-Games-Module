using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMaster : MonoBehaviour
{
    [Serializable]
    public enum FlockThreadMode : byte
    {
        SingleThread,
        Multithreaded
    }

    [SerializeField] private FlockThreadMode flockThreadMode;

    [SerializeField] private old_GlobalFlock old_GlobalFlock;

    [SerializeField] private GlobalFlock globalFlock;

    private void Awake()
    {
        if (flockThreadMode == FlockThreadMode.SingleThread)
        {
            this.globalFlock = null;
            old_GlobalFlock.Awake();
        }
        else
        {
            this.old_GlobalFlock = null;
        }
    }

    private void Start()
    {
        if (flockThreadMode == FlockThreadMode.SingleThread)
        {
            this.old_GlobalFlock.SpawnBird = Instantiate;
            old_GlobalFlock.Start();
        }
        else
        {
            this.globalFlock.SpawnBird = Instantiate;
            this.globalFlock.Start();
        }
    }


    private void Update()
    {
        if (flockThreadMode == FlockThreadMode.SingleThread)
        {
            this.old_GlobalFlock.Update();
        }
        else
        {
            this.globalFlock.Update();
        }
    }


    private void OnDestroy()
    {
        old_GlobalFlock.instance = null;

        if (this.flockThreadMode == FlockThreadMode.SingleThread)
            globalFlock?.OnDestroy();
    }
}