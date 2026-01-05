using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInputValues))]
public class PlayerCameraRotator : MonoBehaviour
{
    //Attributes

    [Header("Dependencies")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;



    [Header("Configurables")]
    public float lookSensitivity = 1.0f;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;



    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;


    //Dependencies resolved in start method
    private PlayerInput _playerInput;
    private PlayerInputValues _input;


    //internal data 
    private const float _threshold = 0.01f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;


    private void Start()
    {
        this._cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        this._playerInput = GetComponent<PlayerInput>();
        this._input = GetComponent<PlayerInputValues>();
    }

    private void LateUpdate()
    {
        bool inputDetected = _input.look.sqrMagnitude >= _threshold;
        if (inputDetected && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse() ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * this.lookSensitivity;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * this.lookSensitivity;
        }

        // clamp our rotations so our values are limited 360 degrees
        ClampPitchAndYaw();

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }


    private bool IsCurrentDeviceMouse()
    {
        return _playerInput.currentControlScheme == "KeyboardMouse";
    }
    private void ClampPitchAndYaw()
    {
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
