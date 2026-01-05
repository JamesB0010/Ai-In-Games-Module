using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_Idle : MonoBehaviour, I_TransitionEvaluator
{
    [SerializeField] private Mouse mouse;
    
    [SerializeField] private float rotationSpeed;
    
    [Tooltip(
            "If the target is within thr possessionRange then the mouse will idle. if the target is outside the possession range then the mouse will chase")]
        [SerializeField]
        private float possessionRange = 15;

        public void EnterState(State state)
    {
        this.mouse.startedWalking?.Invoke();
        transform.rotation = Quaternion.identity;
    }

    public void Behave(State state)
    {
        this.RotateAroundOrigin();
    }

    private void RotateAroundOrigin()
    {
        transform.RotateAround(this.mouse.OriginPoint, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public bool EvaluateTransition(int connectionIndex)
    {
        return Vector3.Distance(transform.position, this.mouse.Target.position) > this.possessionRange;
    }
}
