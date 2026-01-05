//Credit code monkey https://www.youtube.com/watch?v=FbM4CkqtOuA&t=1572s
//Refactored by James Bland
using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using UnityEngine;

[RequireComponent(typeof(PlayerInputValues))]
[RequireComponent(typeof(CrosshairTargetFinder))]
public class OrientBodyWhenAiming : MonoBehaviour
{
    //dependencies resolved in the awake function
    private PlayerInputValues inputs;
    private CrosshairTargetFinder targetFinder;


    //Methods
    private void Awake()
    {
        this.inputs = GetComponent<PlayerInputValues>();
        this.targetFinder = GetComponent<CrosshairTargetFinder>();
    }

    private void Update()
    {
        //if need be aim the player character towards the point they are aiming at
        TryAimPlayerBody();
    }

    private void TryAimPlayerBody()
    {
        bool aiming = this.inputs.aim;
        if (aiming)
            RotatePlayerToFaceAimPoint();
    }

    private void RotatePlayerToFaceAimPoint()
    {
        Vector3 aimDirection = FindAimDirectionHorizontal();

        //do the rotating
        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20.0f);
    }

    private Vector3 FindAimDirectionHorizontal()
    {
        //the horizontal in the method name refers to the fact the y component of the world aim target is the same as the y component of the player characters position
        //this is done so that when we set the player characters forward vector to match the aim direction the players feet stay 
        //on the ground and they aren't rotated in a weird way
        Vector3 worldAimTarget = this.targetFinder.GetLatestHitPosition();
        worldAimTarget.y = transform.position.y;

        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
        return aimDirection;
    }
}




