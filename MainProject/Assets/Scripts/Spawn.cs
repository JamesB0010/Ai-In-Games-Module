using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject patientPrefab;

    [SerializeField] private int numInitialPatients, minDurationSpawn, maxDurationSpawn;

    private void Start()
    {
        for (int i = 0; i < this.numInitialPatients; ++i)
        {
            Vector3 location = new Vector3();
            location.x = transform.position.x + Random.Range(-10, 10);
            location.z = transform.position.z + Random.Range(-10, 10);
            location.y = 1.55f;
            Instantiate(this.patientPrefab, location, Quaternion.identity);
        }
        Invoke(nameof(SpawnPatient), 1);
    }

    private void SpawnPatient()
    {
        Vector3 location = new Vector3();
        location.x = transform.position.x + Random.Range(-10, 10);
        location.z = transform.position.z + Random.Range(-10, 10);
        location.y = 1.55f;
        Instantiate(this.patientPrefab, location, Quaternion.identity);
        Invoke(nameof(this.SpawnPatient), Random.Range(minDurationSpawn, maxDurationSpawn));
    }
}
