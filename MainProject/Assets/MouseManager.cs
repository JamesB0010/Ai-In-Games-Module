using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    [SerializeField] public UnityEvent mouseCanBeRidden;

    [SerializeField] public UnityEvent mouseCanNotBeRidden;

    [SerializeField] public UnityEvent mouseMounted;

    [SerializeField] public UnityEvent mouseDismounted;

    private bool readyToRideMouse;
    private RideMouse lastRidableMouse;

    public void MouseSpawned(RideMouse rideMouse)
    {
        rideMouse.mouseCanBeRidden = this.OnMouseCanBeRidden;
        rideMouse.mouseCanNotBeRidden = this.MouseCanNotBeRidden;
        rideMouse.PlayerMountedMouse = this.mouseMounted.Invoke;
        rideMouse.PlayerDismountedMouse = this.mouseDismounted.Invoke;
    }

    private void MouseCanNotBeRidden(RideMouse obj)
    {
        this.readyToRideMouse = false;
        this.mouseCanNotBeRidden?.Invoke();
    }

    private void OnMouseCanBeRidden(RideMouse mouse)
    {
        lastRidableMouse = mouse;
        this.readyToRideMouse = true;
        mouseCanBeRidden?.Invoke();
    }

    public void TryToggleRideMouse(GameObject player)
    {
        if (lastRidableMouse == null)
            return;
        
        
        if (this.lastRidableMouse.BeingRidden)
        {
            this.lastRidableMouse.DismountPlayer(player);
            return;
        }
        
        if (this.readyToRideMouse)
        {
            this.lastRidableMouse.Ride(player);
        }
    }
}
