using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LegacyMouseSpawner))]
public class legacyMouseSpawnerEditor : Editor
{
    private LegacyMouseSpawner spawner;

    private void OnEnable()
    {
        this.spawner = target as LegacyMouseSpawner;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (!Application.isPlaying)
            return;
        
        if (GUILayout.Button("Spawn Mouse"))
        {
            this.spawner?.SpawnMouse();
        }
    }
}
