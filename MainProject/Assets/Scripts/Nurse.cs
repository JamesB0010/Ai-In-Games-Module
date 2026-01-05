using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Nurse : Agent
{
    private new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("treatPatient", 1, true);
        this.goals.Add(s1, 3);
    }
}
