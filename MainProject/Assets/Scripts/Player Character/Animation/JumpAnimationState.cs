using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JumpAnimationState : StateMachineBehaviour
{
    public AudioClip jumpAudioClip;
    [Range(0, 1)] public float jumpAudioVolume = 0.5f;

    private Transform playerTransform;

    private void Awake()
    {
        this.playerTransform = FindObjectOfType<OrientBodyWhenAiming>().transform;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioSource.PlayClipAtPoint(this.jumpAudioClip, this.playerTransform.position, this.jumpAudioVolume);
    }
}
