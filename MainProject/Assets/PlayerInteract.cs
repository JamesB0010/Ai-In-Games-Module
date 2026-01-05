using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private MouseManager mouseManager;

    public void OnInteract(InputValue value)
    {
        mouseManager.TryToggleRideMouse(this.gameObject);
    }
}
