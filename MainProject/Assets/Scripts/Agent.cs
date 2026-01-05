using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal //helper class for agent
{
    public Dictionary<string, int> sGoals = new(); //nurse resting (dont need to remove)
    public bool remove; //some agents need to be given a goal, when satisfied remove from their interntions

    public SubGoal(string key, int value, bool removable)
    {
        this.sGoals.Add(key, value);
        this.remove = removable;
    }
}

public class Agent : MonoBehaviour
{
    private List<GOAPAction> actions = new();
    protected Dictionary<SubGoal, int> goals = new();
    private Inventory inventory = new();
    public Inventory Inventory => this.inventory;
    private Planner planner;
    private Queue<GOAPAction> actionQueue;
    private GOAPAction currentAction;
    private SubGoal currentGoal;
    
    protected void Start()
    {
        GOAPAction[] acts = GetComponents<GOAPAction>(); // can drag actions onto the agent
        foreach (var action in acts)
        {
            this.actions.Add(action);
        }
    }

    private bool invoked = false;

    private void CompleteAction()
    {
        this.currentAction.running = false;
        this.currentAction.PostPerform();
        this.invoked = false;
    }
    //late update is called once per frame

    private void LateUpdate()
    {
        //IsAchievable may need to be influenced by other logic - we assume these are all possible
        //need to implement planner here
        //gets goals that need to be satisfied
        //gets a list of actions available to us
        //gets world states

        if (this.currentAction != null && this.currentAction.running)
        {
            if (this.currentAction.agent.hasPath && this.currentAction.agent.remainingDistance < 1f) //navmesh code
            {
                if (!this.invoked)
                {
                    Invoke(nameof(this.CompleteAction), this.currentAction.duration);
                    this.invoked = true;
                }
            }

            return;
        }

        if (this.planner == null || this.actionQueue == null)
        {
            this.planner = new Planner();
            var sortedGoals = from entry in this.goals orderby entry.Value descending select entry;
            foreach (var sortedGoal in sortedGoals)
            {
                this.actionQueue = planner.Plan(this.actions, sortedGoal.Key.sGoals, null);
                if (this.actionQueue != null)
                {
                    this.currentGoal = sortedGoal.Key;
                    break;
                }
            }
        }
        
        //have we done all actions?
        if (this.actionQueue != null && this.actionQueue.Count == 0)
        {
            if (this.currentGoal.remove) //if goal working on is removable
                this.goals.Remove(this.currentGoal);

            this.planner = null;
        }

        if (this.actionQueue != null && this.actionQueue.Count > 0) //if action queue and we still have actions to do
        {
            this.currentAction = this.actionQueue.Dequeue();
            if (this.currentAction.PrePerform()) //check cubicles available etc...
            {
                if (this.currentAction.target == null && this.currentAction.targetTag != "")
                    this.currentAction.target = GameObject.FindWithTag(this.currentAction.targetTag);

                if (this.currentAction.target != null)
                {
                    this.currentAction.running = true; //action starts to take place
                    this.currentAction.agent.SetDestination(this.currentAction.target.transform.position);
                }
            }
            else
            {
                this.actionQueue = null;
            }
        }
    }
}
