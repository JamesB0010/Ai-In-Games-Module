using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LegacyMouseSpawner : MonoBehaviour
{
    [FormerlySerializedAs("legacyMouseMousePrefab")] [FormerlySerializedAs("mousePrefab")] [SerializeField] private LegacyMouse legacyMousePrefab;

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
        Vector3 mouseSpawnPos = hit.point + Vector3.up * legacyMousePrefab.YOffset;
        LegacyMouse legacyMouseInstance = Instantiate(this.legacyMousePrefab, mouseSpawnPos, Quaternion.LookRotation(this.target.position - mouseSpawnPos, Vector3.up));
        legacyMouseInstance.Target = this.target;
        legacyMouseInstance.home = this.mouseHome;
        legacyMouseInstance.houseSleepSpot = this.sleepSpot;
    }
}
