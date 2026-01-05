using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BTNode : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    [HideInInspector] public Blackboard blackboard;

    [HideInInspector] public AIAgent agent;
    private State state = State.Running;
    private bool started = false;
    [HideInInspector] public string guid;

    [HideInInspector] public Vector2 GraphEditorPosition = new Vector2();

    [TextArea] public string description;

    public State NodeState => this.state;
    public bool Started => this.started;

    public State Update()
    {
        if (!this.started)
        {
            this.OnStart();
            this.started = true;
        }

        state = OnUpdate();

        if (this.state == State.Failure || this.state == State.Success)
        {
            this.OnStop();
            this.started = false;
        }

        return this.state;
    }

    public virtual BTNode Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
