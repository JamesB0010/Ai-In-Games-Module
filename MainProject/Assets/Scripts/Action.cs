using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public abstract class GOAPAction : MonoBehaviour //pertains to the details of the actions
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target; //Location where action will happen
    public string targetTag; // pickup gameobject using tag - if exist in heirarchy
    public float duration = 0; //how long will this action take?
    public WorldState[] preConditions; //get via inspector (Serializable) - put into dictionaries
    public WorldState[] afterEffects;
    public NavMeshAgent agent; // attached to agent for movement
    public Dictionary<string, int> preconditions = new();
    public Dictionary<string, int> effects = new();
    public WorldStates agentBeliefs;
    public bool running = false; //default false, need to know if were running the action

    protected Inventory inventory;

    private void Awake()
    {
        this.agent = this.gameObject.GetComponent<NavMeshAgent>();
        if (this.preConditions != null)
        {
            foreach (WorldState w in this.preConditions)
            {
                this.preconditions.Add(w.key, w.value);
            }
        }
        if(this.afterEffects != null)
        {
            foreach (WorldState w in this.afterEffects)
            {
                this.effects.Add(w.key, w.value);
            }
        }
        
        this.inventory = this.GetComponent<Agent>().Inventory;
    }

    public bool IsAchievable()
    {
        return true; //Change later - assume true for now
    }
    
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (var precondition in this.preconditions)
        {
            if (!conditions.ContainsKey(precondition.Key))
                return false;
        }

        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
