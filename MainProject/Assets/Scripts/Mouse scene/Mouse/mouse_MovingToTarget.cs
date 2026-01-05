using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_MovingToTarget : MonoBehaviour
{
    [SerializeField] private Mouse mouse;
    public void EnterState(State state)
    {
        mouse.MoveToTarget(mouse.Target);
    }

    public void Behave(State state)
    {
        if(this.mouse.Target.position != mouse.TargetPosition)
            this.mouse.MoveToTarget(mouse.Target);
    }
}
