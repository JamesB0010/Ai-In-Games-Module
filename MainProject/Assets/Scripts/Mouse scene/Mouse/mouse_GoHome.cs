using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_GoHome : MonoBehaviour
{
    [SerializeField] private Mouse mouse;
    
    
    [HideInInspector] public Transform home;

    private bool enteringHouse;
    
    public void EnterState(State state)
    {
        this.mouse.startedWalking?.Invoke();
        this.mouse.MoveToTarget(home);
    }

    public void Behave(State state)
    {
        if (Vector3.Distance(transform.position, this.home.position) <= 10 && !this.enteringHouse)
        {
            this.enteringHouse = true;
            this.mouse.EnterMouseHouse();
        }
    }

    public void ExitState(State state)
    {
        this.enteringHouse = false;
    }
}
