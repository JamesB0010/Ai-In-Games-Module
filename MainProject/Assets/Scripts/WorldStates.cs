using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

//Serializable wrapper class for a key value pair
[Serializable]
public class WorldState
{
    public string key;
    public int value;
}

public class WorldStates
{
    private Dictionary<string, int> states;
    public Dictionary<String, int> States => this.states;

    public WorldStates()
    {
        this.states = new();
    }

    public bool HasState(string key)
    {
        return this.states.ContainsKey(key);
    }

    private void AddState(string key, int value)
    {
        this.states.Add(key,value);
    }

    public void ModifyState(string key, int value)
    {
        if (this.states.ContainsKey(key))
        {
            this.states[key] += value;
            if (this.states[key] <= 0)
                this.RemoveState(key);
        }
        else
        {
            this.states.Add(key, value);
        }
    }

    public void RemoveState(string key)
    {
        if (this.states.ContainsKey(key))
            this.states.Remove(key);
    }

    public void SetState(string key, int value)
    {
        if (this.states.ContainsKey(key))
            this.states[key] = value;
        else
            this.states.Add(key, value);
    }
}
