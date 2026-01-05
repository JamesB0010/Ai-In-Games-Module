using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class RideMouse : MonoBehaviour
{
    public Action<RideMouse> mouseCanBeRidden;

    public Action<RideMouse> mouseCanNotBeRidden;

    public Action PlayerMountedMouse;

    public Action PlayerDismountedMouse;

    private bool beingRidden = false;

    [SerializeField] private SkinnedMeshRenderer playerRidingModel;

    [SerializeField] private Transform ejectPoint;

    public bool BeingRidden => this.beingRidden;

    [SerializeField] private CinemachineVirtualCamera rideCam;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UnityEngine.CharacterController c))
        {
            mouseCanBeRidden?.Invoke(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out UnityEngine.CharacterController c))
        {
            mouseCanNotBeRidden?.Invoke(this);
        }
    }

    public void Ride(GameObject player)
    {
        this.beingRidden = true;
        this.rideCam.gameObject.SetActive(true);
        this.playerRidingModel.enabled = true;
        this.PlayerMountedMouse.Invoke();
    }

    public void DismountPlayer(GameObject player)
    {
        this.PlayerDismountedMouse.Invoke();
        this.beingRidden = false;
        this.rideCam.gameObject.SetActive(false);
        this.playerRidingModel.enabled = false;
        var characterController = player.GetComponent<UnityEngine.CharacterController>();
        characterController.enabled = false;
        player.transform.position = this.ejectPoint.position;
        player.transform.rotation = this.ejectPoint.rotation;
        characterController.enabled = true;
        characterController.Move(Vector3.up * 10 + player.transform.forward * 5);
    }
}
