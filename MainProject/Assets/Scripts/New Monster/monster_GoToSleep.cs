using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_GoToSleep : MonoBehaviour
{
    public void EnteredState(State state)
    {
        GetComponent<Animator>().SetTrigger("GoToSleep");
    }
}
