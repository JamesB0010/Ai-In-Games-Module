using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is an example of how the finite state machine can be used
public class CubeBehaviours : FSMBehaviour
{
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float JumpHeight;

    [SerializeField] private float timeBetweenJumps;

    private float lastJumpTimestamp = -1000;

    [SerializeField] private float tired = 0;

    [SerializeField]
    private State idleState, jumpState;

    [SerializeField] private SuperBaseScriptableValRef excitement;

    private Rigidbody rb;
    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        if (excitement is BoolReference boolRef)
            boolRef.SetValue(false);

        if (excitement is FloatReference floatRef)
            floatRef.SetValue(0);
    }

    public void OnStateIdle()
    {
        transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * this.rotationSpeed);
    }

    public void OnStateJump()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);

        if (hit.distance <= 0.51f && Time.timeSinceLevelLoad - this.lastJumpTimestamp >= this.timeBetweenJumps)
        {
            this.rb.AddForce(new Vector3(0, this.JumpHeight, 0), ForceMode.Impulse);
            this.lastJumpTimestamp = Time.timeSinceLevelLoad;
        }
    }

    public override void Behave(State state)
    {
        if (state.StateName == this.idleState.name)
            OnStateIdle();
        if (state.StateName == this.jumpState.name)
            OnStateJump();
    }


    public override bool EvaluateTransition(State current, State to)
    {
        if (current.StateName == this.jumpState.name && to.StateName == this.idleState.name)
        {
            return this.tired >= 5.0f;
        }

        return false;
    }
}
