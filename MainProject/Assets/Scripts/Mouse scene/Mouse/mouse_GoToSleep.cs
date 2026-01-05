using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class mouse_GoToSleep : MonoBehaviour
{
    [SerializeField] private Mouse mouse;

    [SerializeField] private UnityEvent GoToSleep;

    public void EnterState(State state)
    {
        this.GoToSleep?.Invoke();
        this.mouse.stoppedWalking?.Invoke();
    }
}
