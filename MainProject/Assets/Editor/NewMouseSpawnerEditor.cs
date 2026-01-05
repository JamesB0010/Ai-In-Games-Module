using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MouseSpawner))]
public class NewMouseSpawnerEditor : Editor
{
    private MouseSpawner spawner;

    private void OnEnable()
    {
        this.spawner = target as MouseSpawner;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!Application.isPlaying)
            return;
        
        if(GUILayout.Button("Spawn Mouse"))
            this.spawner?.SpawnMouse();
    }
}
