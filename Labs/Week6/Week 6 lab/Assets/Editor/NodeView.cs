using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected { get; set; }
    
    private BTNode node;

    public BTNode Node => this.node;

    private Port input;
    private Port output;
    public Port Input => this.input;
    public Port Output => this.output;
    
    public NodeView(BTNode node) : base("Assets/Editor/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name;
        base.viewDataKey = node.guid;
        style.left = node.GraphEditorPosition.x;
        style.top = node.GraphEditorPosition.y;
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/NodeViewStyle.uss");
        base.styleSheets.Add(styleSheet);
        

        this.CreateInputPorts();
        this.CreateOutputPorts();
        this.SetupClasses();

        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    private void SetupClasses()
    {
        switch (this.node)
        {
            case RootNode:
                base.AddToClassList("root");
                break;
            case ActionNode:
                base.AddToClassList("action");
             break;
            case CompositeNode:
                base.AddToClassList("composite");
             break;
            case DecoratorNode:
                base.AddToClassList("decorator");
             break;
        }
    }


    private void CreateInputPorts()
    {
        switch (this.node)
        {
            case RootNode:
                break;
            case ActionNode:
             this.input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
             break;
            case CompositeNode:
             this.input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
             break;
            case DecoratorNode:
             this.input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
             break;
        }

        if (this.input != null)
        {
            this.input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            base.inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        switch (this.node)
        {
            case ActionNode:
                break;
            case CompositeNode:
                this.output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
                break;
            case DecoratorNode:
                this.output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                break;
            case RootNode:
                this.output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                break;
        }
       
        if (output != null)
        {
            this.output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            base.outputContainer.Add(output);
        }
    }



    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        
        Undo.RecordObject(this.node, "Behaviour Tree (Set Position");

        this.node.GraphEditorPosition.x = newPos.xMin;

        this.node.GraphEditorPosition.y = newPos.yMin;
        EditorUtility.SetDirty(this.node);
    }


    public override void OnSelected()
    {
        base.OnSelected();
        this.OnNodeSelected?.Invoke(this);
    }

    public void SortChildren()
    {
        CompositeNode composite = node as CompositeNode;
        if (composite)
        {
            composite.Children.Sort(this.SortByHorizontalPosition);
        }
    }

    private int SortByHorizontalPosition(BTNode left, BTNode right)
    {
        return left.GraphEditorPosition.x < right.GraphEditorPosition.x ? -1 : 1;
    }

    public void UpdateState()
    {
        if (!Application.isPlaying)
            return;
        
        RemoveFromClassList("Running");
        RemoveFromClassList("Failure");
        RemoveFromClassList("Success");
        switch (node.NodeState)
        {
            case BTNode.State.Running:
                if (node.Started)
                    AddToClassList("Running");
                break;
            case BTNode.State.Failure:
                AddToClassList("Failure");
                break;
            case BTNode.State.Success:
                AddToClassList("Success");
                break;
        }
    }
}
