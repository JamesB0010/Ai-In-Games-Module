using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using UnityEngine;

public class JumpAndGravity : AnimatorParameterSetterUtilizer
{
    //Attributes
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;


    private float _verticalVelocity;
    public float VerticalVelocity
    {
        get => this._verticalVelocity;
    }


    [Header("Configurables")]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;


    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    public float _terminalVelocity = 53.0f;



    //Dependencies handled in the start method
    private GroundedChecker groundedChecker;
    private PlayerInputValues _input;


    protected override void Start()
    {
        base.Start();
        this.groundedChecker = GetComponent<GroundedChecker>();
        _input = GetComponent<PlayerInputValues>();

        this._jumpTimeoutDelta = JumpTimeout;
        this._fallTimeoutDelta = FallTimeout;
    }

    protected override List<string> ReturnAnimationIDs()
    {
        return new List<string> { "Jump", "FreeFall" };
    }

    private void Update()
    {
        JumpAndApplyGravity();
    }


    public void JumpAndApplyGravity()
    {
        if (this.groundedChecker.Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            this.animatorParameterSetter.SetAnimatorBool("Jump", false);
            this.animatorParameterSetter.SetAnimatorBool("FreeFall", false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);


                this.animatorParameterSetter.SetAnimatorBool("Jump", true);
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                this.animatorParameterSetter.SetAnimatorBool("FreeFall", true);
            }

            // if we are not grounded, do not jump
            _input.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
}
