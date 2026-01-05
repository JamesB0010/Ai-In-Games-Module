using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GOAPNode
{
    //Creates a set of nodes that are linked
    //there point back to actions based on preconditions and effects
    public GOAPNode parent;
    public float cost;
    public Dictionary<string, int> state; //holds states, world state in root node
    public GOAPAction action; //node points to this action

    public GOAPNode(GOAPNode parent, float cost, Dictionary<string, int> allStates, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates); //copy constructor
        this.action = action;
    }
}
public class Planner 
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GOAPAction> usableActions = new();
        foreach (GOAPAction action in actions)
        {
            if(action.IsAchievable())
                usableActions.Add(action);
        }

        List<GOAPNode> leaves = new List<GOAPNode>();
        GOAPNode start = new GOAPNode(null, 0, World.Instance.GetWorld().States, null);

        bool sucess = this.BuildGraph(start, leaves, usableActions, goal);
        if (!sucess)
        {
            Debug.Log("No plan found, whoops");
            return null; //return no plan if none found
        }

        //Have found a plan so find the cheapest one
        GOAPNode cheapest = null;
        foreach (GOAPNode leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf; //not found anything cheaper yet
            else if (leaf.cost < cheapest.cost)
                cheapest = leaf; //found cheapest leaf

            List<GOAPAction> result = new List<GOAPAction>();
            GOAPNode n = cheapest;
            while (n != null)
            {
                if(n.action != null)
                    result.Insert(0, n.action);

                n = n.parent;
            }

            Queue<GOAPAction> queue = new Queue<GOAPAction>();
            foreach (GOAPAction action in result)
            {
                queue.Enqueue(action);
            }
            
            Debug.Log("The plan is: ");
            foreach (GOAPAction action in queue)
            {
                Debug.Log("Q: " + action.actionName);
            }

            return queue;
        }

        return null;
    }

    private bool BuildGraph(GOAPNode parent, List<GOAPNode> leaves, List<GOAPAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (GOAPAction action in usableActions)
        {
            //future planning
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);

                foreach (var effect in action.effects)
                {
                    if(!currentState.ContainsKey(effect.Key))
                        currentState.Add(effect.Key, effect.Value);
                }

                GOAPNode node = new GOAPNode(parent, parent.cost + action.cost, currentState, action);

                if (this.GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GOAPAction> subset = this.ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);

                    if (found)
                        foundPath = true;
                }
            }
        }

        return foundPath;
    }

    private List<GOAPAction> ActionSubset(List<GOAPAction> actions, GOAPAction removeMe)
    {
        List<GOAPAction> subset = new List<GOAPAction>();

        foreach (GOAPAction action in actions)
        {
            if(!action.Equals(removeMe))
                subset.Add(action);
        }

        return subset;
    }
    
    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (var g in goal)
        {
            if (!state.ContainsKey(g.Key))
                return false;
        }

        return true;
    }
}
