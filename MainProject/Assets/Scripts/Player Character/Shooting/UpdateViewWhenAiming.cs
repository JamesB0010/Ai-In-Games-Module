using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Cinemachine;
using UnityEngine;

//this component is responsible for zooming in the camera and lowering the sensitivity when aiming 
[RequireComponent(typeof(PlayerInputValues))]
[RequireComponent(typeof(PlayerCameraRotator))]
public class UpdateViewWhenAiming : MonoBehaviour
{
    //Attributes
    private float normalSensitivity;


    [Header("Dependencies")]
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;

    [Space]

    [Header("Configurables")]
    [SerializeField]
    private float aimSensitivity;

    //Dependencies resolved in the awake/start function
    private PlayerInputValues inputs;
    private PlayerCameraRotator playerCameraRotator;
    private HorizontalMovement _horizontalMovement;

    //Methods
    private void Awake()
    {
        this.inputs = GetComponent<PlayerInputValues>();
        this.playerCameraRotator = GetComponent<PlayerCameraRotator>();
    }

    private void Start()
    {
        this.normalSensitivity = this.playerCameraRotator.lookSensitivity;
        this._horizontalMovement = GetComponent<HorizontalMovement>();
    }

    private void Update()
    {
        TryAimCamera();
    }

    private void TryAimCamera()
    {
        bool aiming = this.inputs.aim;
        if (aiming)
            this.SetCharacterAimState(true);
        else
            this.SetCharacterAimState(false);
    }

    private void SetCharacterAimState(bool aiming)
    {
        //if we are aiming then we dont want our movement to affect our rotation... because we are aiming
        //in tge same way if we are not aiming then we want to rotate to face the direction we are moving in
        this.aimVirtualCamera.gameObject.SetActive(aiming);
        this._horizontalMovement.SetRotateOnMove(!aiming);
        this.playerCameraRotator.lookSensitivity = aiming ? this.aimSensitivity : this.normalSensitivity;
    }
}
