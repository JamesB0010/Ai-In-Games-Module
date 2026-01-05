using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// not intended to be attached as a component
/// intended to be inherited from
///
/// MAKE SURE you call base.start in any child class' start method
/// then you can override the returnAnimationIDs function to pass a list of parameter ids the script will interact with
/// then you can use the AnimatorParameterSetter to set the parameters 
/// </summary>
public abstract class AnimatorParameterSetterUtilizer : MonoBehaviour
{
    protected AnimatorParameterSetter animatorParameterSetter;

    protected virtual void Start()
    {
        this.animatorParameterSetter = GetComponent<AnimatorParameterSetter>();
        this.animatorParameterSetter.InitiliseParameterIds(this.ReturnAnimationIDs());
    }

    protected abstract List<string> ReturnAnimationIDs();
}
