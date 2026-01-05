using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseGlobalFlock
{
    
    
    [SerializeField] protected Transform player;
    
    
    protected static readonly int airSize = 100;
        
    [SerializeField] protected int numBirds = 300;

    protected Vector3 goalPos = Vector3.zero;

    public Vector3 GoalPos => this.goalPos;
    
    
    public static float AirSize => old_GlobalFlock.airSize;
    
    protected Transform[] birds;
    
    public Func<Transform, Vector3, Quaternion, Transform, Transform> SpawnBird;
}
