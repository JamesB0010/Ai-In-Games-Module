using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    [SerializeField] private float duration = 1;
    private float startTime;
    protected override void OnStart()
    {
        this.startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Time.time - this.startTime > this.duration)
            return State.Success;

        return State.Running;
    }
}
