using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    [SerializeField] private string message;

    public string Message
    {
        get => this.message;
        set => this.message = value;
    }
    protected override void OnStart()
    {
        Debug.Log($"On Start {this.message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"On Stop{this.message}");
    }

    protected override State OnUpdate()
    {
        Debug.Log($"On Update{this.message}");
        return State.Success;
    }

    public override BTNode Clone()
    {
        DebugLogNode node = Instantiate(this);
        node.message = this.message;
        return node;
    }
}
