using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class LinkPathfindingAgentToDebug
{
    private static GridPathRenderer gridPathRenderer = null;

    static LinkPathfindingAgentToDebug()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
        Selection.selectionChanged += TrySetGridPathRendererFocusedAgent;
    }

    private static void TrySetGridPathRendererFocusedAgent()
    {
        if (Selection.activeGameObject != null &&
            Selection.activeGameObject.TryGetComponent(out PathfollowingAgent agent))
        {
            gridPathRenderer?.SetFocusedAgent(agent);
        }
    }

    private static void PlayModeStateChanged(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.EnteredPlayMode)
        {
            gridPathRenderer = GameObject.FindObjectOfType<GridPathRenderer>();
        }
    }
}