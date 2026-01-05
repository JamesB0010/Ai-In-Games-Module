using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(FlockMaster))]
public class FlockMasterEditor : Editor
{
    static FlockMasterEditor()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
    }

    private static void PlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SetGuiEnabled(false);
        }

        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            SetGuiEnabled(true);
        }
    }

    private static void SetGuiEnabled(bool value)
    {
        threadModeDropdown?.SetEnabled(value);
        oldGlobalFlockField?.SetEnabled(value);
        globalFlockField?.SetEnabled(value);
    }

    public VisualTreeAsset visualTree;
    private static EnumField threadModeDropdown;
    private static PropertyField oldGlobalFlockField;
    private static PropertyField globalFlockField;


    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        visualTree.CloneTree(root);
        
        root.Q<ObjectField>("m_Script").SetEnabled(false);

        oldGlobalFlockField = root.Q<PropertyField>("oldGlobalFlock");

        globalFlockField = root.Q<PropertyField>("globalFlock");

        threadModeDropdown = root.Q<EnumField>("flockThreadMode");
        threadModeDropdown.RegisterValueChangedCallback(this.ThreadModeChanged);

        var startThreadMode = (FlockMaster.FlockThreadMode)serializedObject.FindProperty("flockThreadMode").intValue;
        
        ConditionallyShowHidePropertyFields(startThreadMode);
        
        FlockMasterEditor.SetGuiEnabled(!EditorApplication.isPlaying);
        
        return root;
    }

    private void ThreadModeChanged(ChangeEvent<Enum> evt)
    {
        var threadMode = (FlockMaster.FlockThreadMode)evt.newValue;
        this.ConditionallyShowHidePropertyFields(threadMode);
    }

    private void ConditionallyShowHidePropertyFields(FlockMaster.FlockThreadMode threadMode)
    {

        if (threadMode == FlockMaster.FlockThreadMode.SingleThread)
        {
            oldGlobalFlockField.style.display = DisplayStyle.Flex;
            globalFlockField.style.display = DisplayStyle.None;
        }
        else
        {
            oldGlobalFlockField.style.display = DisplayStyle.None;
            globalFlockField.style.display = DisplayStyle.Flex;
        }
    }
}
