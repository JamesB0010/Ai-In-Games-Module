using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeEditor : EditorWindow
{
    private BehaviourTreeView treeView;
    private InspectorView inspectorView;
    private IMGUIContainer blackboardview;

    private SerializedObject treeObject;
    private SerializedProperty blackboardProperty;
    
    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        
        //Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        this.treeView = root.Q<BehaviourTreeView>();

        this.inspectorView = root.Q<InspectorView>();

        this.blackboardview = root.Q<IMGUIContainer>();

        blackboardview.onGUIHandler = () =>
        {
            treeObject.Update();
            EditorGUILayout.PropertyField(blackboardProperty);
            treeObject.ApplyModifiedProperties();
        };

        this.treeView.OnNodeSelected = OnNodeSelectionChanged;
        
        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
        }
    }


    private void OnSelectionChange()
    {
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                if (Selection.activeGameObject.TryGetComponent(out BehaviourTreeRunner btRunner))
                {
                    tree = btRunner.Tree;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                if(this.treeView != null)
                    this.treeView.PopulateView(tree);   
            }

        }
        else
        {
            if (tree != null && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                this.treeView.PopulateView(tree);
        }

        if (tree != null)
        {
            this.treeObject = new SerializedObject(tree);
            this.blackboardProperty = treeObject.FindProperty("blackboard");
        }
    }

    private NodeView lastSelected = null;
    private void OnNodeSelectionChanged(NodeView nodeView)
    {
        this.inspectorView.UpdateSelection(nodeView);
        lastSelected?.RemoveFromClassList("Selected");
        nodeView.AddToClassList("Selected");
        this.lastSelected = nodeView;
        nodeView.MarkDirtyRepaint();
    }

    private void OnInspectorUpdate()
    {
        treeView.UpdateNodeStates();
    }
}
