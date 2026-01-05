using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_Chewing : MonoBehaviour, I_TransitionEvaluator
{
    private float startedChewingTimestamp;
    
    [SerializeField] private Monster monster;

    private bool EvaluateToHappy()
    {
        return !this.monster.currentFoodItem.IsHealthy();
    }

    private bool EvaluateToAngry()
    {
        return this.monster.currentFoodItem.IsHealthy();
    }

    public void EnterState(State state)
    {
            GetComponent<Animator>().SetTrigger("startChewing");
            this.startedChewingTimestamp = Time.timeSinceLevelLoad;
    }

    public bool EvaluateTransition(int connectionIndex)
    {
        float chewTime = 3f;
        bool chewedForLongEnough = Time.timeSinceLevelLoad - this.startedChewingTimestamp >= chewTime;

        if (!chewedForLongEnough)
            return false;

        if (connectionIndex == 0)
        {
            return this.EvaluateToHappy();
        }

        if (connectionIndex == 1)
        {
            return this.EvaluateToAngry();
        }

        return false;
    }
}
