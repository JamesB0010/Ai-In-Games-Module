using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : BTNode
{
    [SerializeField] protected BTNode child;

    public BTNode Child
    {
        get => this.child;
        set => this.child = value;
    }
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return child.Update();
    }

    public override BTNode Clone()
    {
        RootNode node = Instantiate(this);
        node.child = this.child.Clone();
        return node;
    }
}
