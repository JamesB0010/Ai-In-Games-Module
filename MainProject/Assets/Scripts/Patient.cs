using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Patient : Agent
{
    private new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        this.goals.Add(s1, 3);
    }
}
