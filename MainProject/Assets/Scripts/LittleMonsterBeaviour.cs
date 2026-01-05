using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleMonsterBeaviour : FSMBehaviour
{
    [Header("States")] [SerializeField] private State waitingToBeFed, openMouthState, chewingState, happyState, angryState;

    private I_FoodItem currentFoodItem;
    
    [SerializeField] private MonsterMouth monsterMouth;

    private float startedChewingTimestamp;

    private float emotionStateEnterTime;

    [SerializeField] private MeshRenderer eye;
    private float eyeColorChangeTime;

    private void Start()
    {
        monsterMouth.foodInMouth += this.FoodInMouth;
    }

    private void FoodInMouth(I_FoodItem food)
    {
        var agentFSM = GetComponent<FSMMonoComponent>().FiniteStateMachine;
        if (agentFSM.ActiveState.IsInstanceOf(this.openMouthState))
        {
            this.currentFoodItem = food;
            agentFSM.ActiveState.Transition();
        }
    }

    public override void EnterState(State state)
    {
        if (state.IsInstanceOf(this.openMouthState))
        {
            this.ChangeEyeColour(Color.white);
            GetComponent<Animator>().SetTrigger("OpenMouth");
        }

        if (state.IsInstanceOf(this.chewingState))
        {
            GetComponent<Animator>().SetTrigger("startChewing");
            this.startedChewingTimestamp = Time.timeSinceLevelLoad;
        }


        if (state.IsInstanceOf(this.happyState))
        {
            GetComponent<Animator>().SetTrigger("HappyReaction");
            this.ChangeEyeColour(Color.green);
            this.emotionStateEnterTime = Time.timeSinceLevelLoad;
            this.currentFoodItem.Destroy();
        }

        if (state.IsInstanceOf(this.angryState))
        {
            GetComponent<Animator>().SetTrigger("AngryReaction");
            this.ChangeEyeColour(Color.red);
            this.emotionStateEnterTime = Time.timeSinceLevelLoad;
            this.currentFoodItem.Destroy();
        }
    }

    private void ChangeEyeColour(Color color)
    {
        eyeColorChangeTime = 1.0f;
        this.eye.material.color.LerpTo(color, eyeColorChangeTime, val => this.eye.material.color = val);
    }
    
    public override bool EvaluateTransition(State current, State to)
    {
        if (current.IsInstanceOf(this.chewingState))
        {
            float chewTime = 3f;
            bool chewedForLongEnough = Time.timeSinceLevelLoad - this.startedChewingTimestamp >= chewTime;
            
            if (!chewedForLongEnough)
                return false;
        }


        if (to.IsInstanceOf(this.happyState))
        {
            return !this.currentFoodItem.IsHealthy();
        }else if (to.IsInstanceOf(this.angryState))
        {
            return this.currentFoodItem.IsHealthy();
        }


        if (to.IsInstanceOf(this.waitingToBeFed))
        {
            float timeSinceEnteredEmotionState = Time.timeSinceLevelLoad - this.emotionStateEnterTime;
            return timeSinceEnteredEmotionState > this.eyeColorChangeTime + 1;
        }
        
        return false;
    }
}
