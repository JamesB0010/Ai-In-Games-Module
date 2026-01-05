using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunRotator : MonoBehaviour
{
    [SerializeField] private Transform directionalLight;

    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            //rotate sun
            
            this.directionalLight.rotation *= Quaternion.Euler(this.rotationSpeed * Time.deltaTime, 0,0 );
            return;
        }

        if (Input.GetKey(KeyCode.X))
        {
            //rotate sun
            this.directionalLight.rotation *= Quaternion.Euler(-this.rotationSpeed * Time.deltaTime, 0,0 );
            return;
        }
    }
}
