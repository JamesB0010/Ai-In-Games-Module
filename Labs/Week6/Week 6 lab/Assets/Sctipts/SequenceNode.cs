using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    private int current;
    
    
    protected override void OnStart()
    {
        this.current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        BTNode child = base.children[this.current];

        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                this.current++;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return current == base.children.Count ? State.Success : State.Running;
    }
}
