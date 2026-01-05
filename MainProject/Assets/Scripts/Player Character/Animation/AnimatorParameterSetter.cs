using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class AnimatorParameterSetter : MonoBehaviour
{
    private Dictionary<string, int> animationParameterIDs = new Dictionary<string, int>();

    private bool _hasAnimator;
    private Animator animator;


    private void Start()
    {
        this._hasAnimator = TryGetComponent(out animator);
    }

    private void Update()
    {
        this._hasAnimator = TryGetComponent(out this.animator);
    }

    public void InitiliseParameterIds(List<String> animationIdStrings)
    {
        foreach (string animationId in animationIdStrings)
        {
            if (this.animationParameterIDs.ContainsKey(animationId))
                continue;


            int animIdHash = Animator.StringToHash(animationId);
            this.animationParameterIDs.Add(animationId, animIdHash);
        }
    }

    public void SetAnimatorBool(string animIdName, bool value)
    {
        if (this._hasAnimator == false)
            return;

        Assert.IsTrue(this.animationParameterIDs.ContainsKey(animIdName));

        this.animator.SetBool(this.animationParameterIDs[animIdName], value);
    }

    public void SetAnimatorFloat(string animIdName, float value)
    {
        if (this._hasAnimator == false)
            return;

        Assert.IsTrue(this.animationParameterIDs.ContainsKey(animIdName));

        this.animator.SetFloat(this.animationParameterIDs[animIdName], value);
    }
}
