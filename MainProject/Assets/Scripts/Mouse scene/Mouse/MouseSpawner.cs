using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseSpawner : MonoBehaviour
{
    [SerializeField] private Mouse mousePrefab;

    [SerializeField] private MouseManager mouseManager;

    [Header("Dependencies for mouse")]
    [SerializeField] private Transform mouseHome;

    [SerializeField] private Transform target;

    [SerializeField] private Transform sleepSpot;


    [Header("Things Dependent on mouse")] [SerializeField]
    private GridPathRenderer gridPathRenderer;

    private void Start()
    {
        this.SpawnMouse();
    }

    public void SpawnMouse()
    {
        Physics.Raycast(transform.position + Vector3.up * 10000, Vector3.down, out RaycastHit hit);
        Vector3 mouseSpawnPos = hit.point + Vector3.up * mousePrefab.YOffset;
        Mouse mouseInstance = Instantiate(this.mousePrefab, mouseSpawnPos, Quaternion.LookRotation(this.target.position - mouseSpawnPos, Vector3.up));
        mouseInstance.Target = this.target;
        mouseInstance.GetComponent<mouse_GoHome>().home = this.mouseHome;
        mouseInstance.houseSleepSpot = this.sleepSpot;
        
        if(this.mouseManager != null)
            this.mouseManager.MouseSpawned(mouseInstance.GetComponentInChildren<RideMouse>());
    }
}