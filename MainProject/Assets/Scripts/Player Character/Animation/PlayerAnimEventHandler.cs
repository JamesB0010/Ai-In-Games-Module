using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(UnityEngine.CharacterController))]
public class PlayerAnimEventHandler : MonoBehaviour
{
    //Attributes
    [Header("Sounds and Settings")]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;


    //Dependencies handled in start method
    private UnityEngine.CharacterController characterController;

    private void Start()
    {
        this.characterController = GetComponent<UnityEngine.CharacterController>();
    }


    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight <= 0.5f)
            return;

        if (this.FootstepAudioClips.Length <= 0)
            return;


        var index = Random.Range(0, FootstepAudioClips.Length);
        AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(characterController.center), FootstepAudioVolume);
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight <= 0.5f)
            return;

        AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(characterController.center), FootstepAudioVolume);
    }
}
