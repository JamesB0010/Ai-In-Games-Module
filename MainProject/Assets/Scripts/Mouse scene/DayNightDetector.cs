using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DayNightDetector : MonoBehaviour
{
    [SerializeField] private BoolReference daytime;

    [SerializeField] private Transform sun;

    private void Start()
    {
        this.daytime.SetValue(this.isDaytime());
    }

    private void Update()
    {
        this.daytime.SetValue(this.isDaytime());
    }

    private bool isDaytime()
    {
        float dot = Vector3.Dot(sun.forward, Vector3.down);
    
        // If the sun's forward direction points downward, it's night
        return dot > 0;
    }
}
