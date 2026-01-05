using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class mouse_Sleep : MonoBehaviour
{
    [SerializeField] private UnityEvent wakeUp;
    public void ExitState(State state)
    {
        this.wakeUp?.Invoke();
    }
}
