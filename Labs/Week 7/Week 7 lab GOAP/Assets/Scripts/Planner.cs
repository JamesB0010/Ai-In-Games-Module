using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    //Creates a set of nodes that are linked
    //there point back to actions based on preconditions and effects
    public Node parent;
    public float cost;
    public Dictionary<string, int> state; //holds states, world state in root node
    public Action action; //node points to this action

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Action action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates); //copy constructor
        this.action = action;
    }
}
public class Planner 
{
    public Queue<Action> Plan(List<Action> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<Action> usableActions = new();
        foreach (Action action in actions)
        {
            if(action.IsAchievable())
                usableActions.Add(action);
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, World.Instance.GetWorld().States, null);

        bool sucess = this.BuildGraph(start, leaves, usableActions, goal);
        if (!sucess)
        {
            Debug.Log("No plan found, whoops");
            return null; //return no plan if none found
        }

        //Have found a plan so find the cheapest one
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf; //not found anything cheaper yet
            else if (leaf.cost < cheapest.cost)
                cheapest = leaf; //found cheapest leaf

            List<Action> result = new List<Action>();
            Node n = cheapest;
            while (n != null)
            {
                if(n.action != null)
                    result.Insert(0, n.action);

                n = n.parent;
            }

            Queue<Action> queue = new Queue<Action>();
            foreach (Action action in result)
            {
                queue.Enqueue(action);
            }
            
            Debug.Log("The plan is: ");
            foreach (Action action in queue)
            {
                Debug.Log("Q: " + action.actionName);
            }

            return queue;
        }

        return null;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<Action> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (Action action in usableActions)
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

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (this.GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<Action> subset = this.ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);

                    if (found)
                        foundPath = true;
                }
            }
        }

        return foundPath;
    }

    private List<Action> ActionSubset(List<Action> actions, Action removeMe)
    {
        List<Action> subset = new List<Action>();

        foreach (Action action in actions)
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
