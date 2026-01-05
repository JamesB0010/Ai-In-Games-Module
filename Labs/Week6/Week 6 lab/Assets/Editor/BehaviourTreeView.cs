using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;

public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits>
    {
    }

    public Action<NodeView> OnNodeSelected { get; set; }

    private BehaviourTree tree;
    
    public BehaviourTreeView()
    {
        base.Insert(0, new GridBackground());
        
        
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        base.styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(this.tree);
        AssetDatabase.SaveAssets();
    }


    public void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (this.tree.RootNode == null)
        {
            this.tree.RootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(this.tree);
            AssetDatabase.SaveAssets();
        }

        base.focusable = true;
        //creates node views
        tree.Nodes.ForEach(this.CreateNodeView);
        
        //create edges
        tree.Nodes.ForEach(node =>
        {
            var children = BehaviourTree.GetChildren(node);
            children.ForEach(child =>
            {
                NodeView parentView = this.FindNodeView(node);
                NodeView childView = this.FindNodeView(child);


                Edge edge = parentView.Output.ConnectTo(childView.Input);
                base.AddElement(edge);
            });
        });
    }

    NodeView FindNodeView(BTNode node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }
    
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return base.ports.ToList().Where(endPort =>
            endPort.direction != startPort.direction &&
            endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        graphViewChange.elementsToRemove?.ForEach(elem =>
        {
            NodeView nodeView = elem as NodeView;
            if (nodeView != null)
            {
                tree.DeleteNode(nodeView.Node);
            }

            Edge edge = elem as Edge;
            if (edge != null)
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;

                tree.RemoveChild(parentView.Node, childView.Node);
            }
        });

        graphViewChange.edgesToCreate?.ForEach(edge =>
        {
            NodeView parentView = edge.output.node as NodeView;
            NodeView childView = edge.input.node as NodeView;

            tree.AddChild(parentView.Node, childView.Node);
        });

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach(node =>
            {
                NodeView view = node as NodeView;
                view.SortChildren();
            });
        }

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType?.Name}] {type.Name}", action => this.CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType?.Name}] {type.Name}", action => this.CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType?.Name}] {type.Name}", action => this.CreateNode(type));
            }
        }
    }

    void CreateNode(System.Type type)
    {
        BTNode node = tree.CreateNode(type);
        this.CreateNodeView(node);
    }
    private void CreateNodeView(BTNode node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected; 
        AddElement(nodeView);
    }

    public void UpdateNodeStates()
    {
        nodes.ForEach(node =>
        {
            NodeView view = node as NodeView;
            view.UpdateState();
        });
    }
}
