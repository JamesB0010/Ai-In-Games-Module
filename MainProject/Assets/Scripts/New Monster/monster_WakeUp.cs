using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_WakeUp : MonoBehaviour
{
    public void EnteredState(State state)
    {
        GetComponent<Animator>().SetTrigger("WakeUp");
    }
}
