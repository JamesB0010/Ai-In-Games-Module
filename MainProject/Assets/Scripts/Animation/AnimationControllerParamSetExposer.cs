using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationControllerParamSetExposer : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    public void SetBoolParamTrue(string parameterName)
    {
        this.animator.SetBool(parameterName, true);
    }
    
    public void SetBoolParamFalse(string parameterName)
    {
        this.animator.SetBool(parameterName, false);
    }
}

