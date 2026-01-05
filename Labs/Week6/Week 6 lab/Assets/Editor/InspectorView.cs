using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, InspectorView.UxmlTraits>
    {
        
    }

    private Editor editor;
    public InspectorView()
    {
        
    }

    public void UpdateSelection(NodeView nodeView)
    {
        base.Clear();
        
        UnityEngine.Object.DestroyImmediate(editor);
        
        this.editor = Editor.CreateEditor(nodeView.Node);

        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (editor.target)
            {
                editor.OnInspectorGUI();
            }
        });
        
        Add(container);
    }
}
