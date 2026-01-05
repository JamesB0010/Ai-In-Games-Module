using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_Angry : MonoBehaviour, I_TransitionEvaluator
{
    [SerializeField] private Monster monster;

    public void EnterState(State state)
    {
        GetComponent<Animator>().SetTrigger("AngryReaction");
        this.monster.ChangeEyeColour(Color.red);
        this.monster.emotionStateEnterTime = Time.timeSinceLevelLoad;
        this.monster.currentFoodItem.Destroy();
    }

    public bool EvaluateTransition(int connectionIndex)
    {
        float timeSinceEnteredEmotionState = Time.timeSinceLevelLoad - this.monster.emotionStateEnterTime;
        return timeSinceEnteredEmotionState > this.monster.eyeColorChangeTime + 1;
    }
}
