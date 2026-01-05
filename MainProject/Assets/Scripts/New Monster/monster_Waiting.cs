using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_Waiting : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private MonsterMouth monsterMouth;

    private State state;
    
    private void Start()
    {
        monsterMouth.foodInMouth += this.FoodInMouth;
        this.ResetVisuals();
    }

    private void FoodInMouth(I_FoodItem foodItem)
    {
        if (this.state.active)
        {
            this.monster.currentFoodItem = foodItem;
            this.state.Transition();
        }
    }
    public void EnterState(State state)
    {
        this.state = state;
        this.ResetVisuals();
    }

    private void ResetVisuals()
    {
        this.OpenMouth();
        this.ResetEye();
    }

    private void ResetEye()
    {
        monster.ChangeEyeColour(Color.white);
    }

    private void OpenMouth()
    {
        GetComponent<Animator>().SetTrigger("OpenMouth");
    }
}
