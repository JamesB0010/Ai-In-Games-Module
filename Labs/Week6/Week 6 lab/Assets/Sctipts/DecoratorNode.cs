using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class DecoratorNode : BTNode
{ 
    [SerializeField] protected BTNode child;

    public BTNode Child
    {
        get => this.child;
        set => this.child = value;
    }

    public override BTNode Clone()
    {
        DecoratorNode node = Instantiate(this);
        node.child = this.child.Clone();
        return node;
    }
}
