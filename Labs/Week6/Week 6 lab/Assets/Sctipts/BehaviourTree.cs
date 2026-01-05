using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    private BTNode rootNode;

    public Blackboard blackboard = new Blackboard();
    
    private BTNode.State treeState = BTNode.State.Running;

    [SerializeField] private List<BTNode> nodes = new();

    public List<BTNode> Nodes => this.nodes;
    
    
    public BTNode RootNode
    {
        get => this.rootNode;
        set => this.rootNode = value;
    }

    public BTNode.State Update()
    {
        if(this.rootNode.NodeState == BTNode.State.Running)
            this.treeState = this.rootNode.Update();

        return this.treeState;
    }

    public BTNode CreateNode(System.Type type)
    {
        BTNode node = ScriptableObject.CreateInstance(type) as BTNode;
        node.name = type.ToString();
        node.guid = GUID.Generate().ToString();
        
        Undo.RecordObject(this, "Behaviour Tree (Create Node)");
        this.nodes.Add(node);

        if (!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }
        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create Node)");
        AssetDatabase.SaveAssets();

        return node;
    }

    public void DeleteNode(BTNode node)
    {
        Undo.RecordObject(this, "Behaviour Tree (Delete Node)");
        nodes.Remove(node);
        
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(BTNode parent, BTNode child)
    {
        switch (parent)
        {
            case RootNode rootNode:
                Undo.RecordObject(rootNode, "Behaviour Tree (Add Child)");
                rootNode.Child = child;
                EditorUtility.SetDirty(rootNode);
                break;
            case CompositeNode compositeNode:
                Undo.RecordObject(compositeNode, "Behaviour Tree (Add Child)");
                compositeNode.AddChild(child);
                EditorUtility.SetDirty(compositeNode);
                break;
            case DecoratorNode decoratorNode:
                Undo.RecordObject(decoratorNode, "Behaviour Tree (Add Child)");
                decoratorNode.Child = child;
                EditorUtility.SetDirty(decoratorNode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(parent));
        }
    }

    public void RemoveChild(BTNode parent, BTNode child)
    {
        switch (parent)
        {
            case RootNode rootNode:
                Undo.RecordObject(rootNode, "Behaviour Tree (Remove Child)");
                rootNode.Child = null;
                EditorUtility.SetDirty(rootNode);
                break;
            case CompositeNode compositeNode:
                Undo.RecordObject(compositeNode, "Behaviour Tree (Remove Child)");
                compositeNode.RemoveChild(child);
                EditorUtility.SetDirty(compositeNode);
                break;
            case DecoratorNode decoratorNode:
                Undo.RecordObject(decoratorNode, "Behaviour Tree (Remove Child)");
                decoratorNode.Child = null;
                EditorUtility.SetDirty(decoratorNode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(parent));
        }
    }

    public static List<BTNode> GetChildren(BTNode parent)
    {
        List<BTNode> children = new();
        switch (parent)
        {
            case RootNode rootNode:
                if(rootNode.Child != null)
                    children.Add(rootNode.Child);
                break;
            case CompositeNode compositeNode:
                return compositeNode.Children;
            case DecoratorNode decoratorNode:
                if (decoratorNode.Child != null)
                    children.Add(decoratorNode.Child);
                break;
        }

        return children;
    }

    public void Traverse(BTNode node, System.Action<BTNode> visitor)
    {
        if (node)
        {
            visitor.Invoke(node);
            var children = GetChildren(node);
            children.ForEach(child => Traverse(child, visitor));
        }
    }
    
    public void Bind(AIAgent agent){
        Traverse(rootNode, node =>
        {
            node.agent = agent;
            node.blackboard = blackboard;
        });
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = this.rootNode.Clone();
        tree.nodes = new List<BTNode>();
        Traverse(tree.rootNode, n =>
        {
            tree.nodes.Add(n);
        });
        return tree;
    }
}
