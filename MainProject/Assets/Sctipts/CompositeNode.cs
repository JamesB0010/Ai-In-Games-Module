using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
public abstract class CompositeNode : BTNode
{
    [SerializeField] protected List<BTNode> children = new();

    public List<BTNode> Children => this.children;

    public void AddChild(BTNode child)
    {
        this.children.Add(child);
    }

    public void RemoveChild(BTNode child)
    {
        this.children.Remove(child);
    }

    public override BTNode Clone()
    {
        CompositeNode node = Instantiate(this);
        node.children = this.children.ConvertAll(child => child.Clone());
        return node;
    }
}
