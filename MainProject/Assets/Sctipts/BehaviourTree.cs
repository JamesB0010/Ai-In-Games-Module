using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        #if UNITY_EDITOR
        node.guid = GUID.Generate().ToString();
        
        Undo.RecordObject(this, "Behaviour Tree (Create Node)");
        #endif
        this.nodes.Add(node);

        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }
        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create Node)");
        AssetDatabase.SaveAssets();
#endif
        return node;
    }

    public void DeleteNode(BTNode node)
    {
        #if UNITY_EDITOR
        Undo.RecordObject(this, "Behaviour Tree (Delete Node)");
        #endif
        nodes.Remove(node);
        
        #if UNITY_EDITOR
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
        #endif
    }

    public void AddChild(BTNode parent, BTNode child)
    {
        switch (parent)
        {
            case RootNode rootNode:
                #if UNITY_EDITOR
                Undo.RecordObject(rootNode, "Behaviour Tree (Add Child)");
                #endif
                rootNode.Child = child;
                #if UNITY_EDITOR
                EditorUtility.SetDirty(rootNode);
                #endif
                break;
            case CompositeNode compositeNode:
                #if UNITY_EDITOR
                Undo.RecordObject(compositeNode, "Behaviour Tree (Add Child)");
                #endif
                compositeNode.AddChild(child);
                #if UNITY_EDITOR
                EditorUtility.SetDirty(compositeNode);
                #endif
                break;
            case DecoratorNode decoratorNode:
                #if UNITY_EDITOR
                Undo.RecordObject(decoratorNode, "Behaviour Tree (Add Child)");
                #endif
                decoratorNode.Child = child;
                #if UNITY_EDITOR
                EditorUtility.SetDirty(decoratorNode);
                #endif
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
                #if UNITY_EDITOR
                Undo.RecordObject(rootNode, "Behaviour Tree (Remove Child)");
                rootNode.Child = null;
                #endif
                #if UNITY_EDITOR
                EditorUtility.SetDirty(rootNode);
                #endif
                break;
            case CompositeNode compositeNode:
                #if UNITY_EDITOR
                Undo.RecordObject(compositeNode, "Behaviour Tree (Remove Child)");
                compositeNode.RemoveChild(child);
                #endif
                #if UNITY_EDITOR
                EditorUtility.SetDirty(compositeNode);
                #endif
                break;
            case DecoratorNode decoratorNode:
                #if UNITY_EDITOR
                Undo.RecordObject(decoratorNode, "Behaviour Tree (Remove Child)");
                #endif
                decoratorNode.Child = null;
                #if UNITY_EDITOR
                EditorUtility.SetDirty(decoratorNode);
                #endif
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
