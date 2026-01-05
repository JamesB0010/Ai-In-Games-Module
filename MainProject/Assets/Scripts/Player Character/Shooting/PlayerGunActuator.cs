using System;
using System.Collections;
using System.Collections.Generic;
using CharacterController;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerInputValues))]
[
RequireComponent(typeof(CrosshairTargetFinder))]
public class PlayerGunActuator : MonoBehaviour
{
    //Attributes
    [Header("Dependencies")]
    [SerializeField]
    private Transform bulletBarrelLocation;

    [SerializeField]
    private ParticleSystem muzzleFlashParticleSystem;

    [Space]
    [Header("Configurables")]
    [SerializeField]
    private Gun gun;

    //dependencies resolved in the awake function
    private PlayerInputValues inputs;
    private CrosshairTargetFinder targetFinder;

    //Methods
    private void Awake()
    {
        this.inputs = GetComponent<PlayerInputValues>();
        this.targetFinder = GetComponent<CrosshairTargetFinder>();
    }

    private void Start()
    {
        this.gun.PrimeWeaponToShoot();
    }

    private void Update()
    {
        bool tryingToShoot = this.inputs.shoot;
        if (tryingToShoot)
            TryShootGun();
    }

    private void TryShootGun()
    {
        bool bulletHasBeenShot = false;

        Vector3 crosshairWorldTargetPosition = this.targetFinder.GetLatestHitPosition();


        bool lastHitValid = this.targetFinder.WasLastHitValid();
        if (lastHitValid)
        {
            RaycastHit hit = this.targetFinder.GetLastHit();
            bulletHasBeenShot = this.gun.Shoot(this.bulletBarrelLocation.position, crosshairWorldTargetPosition, true, hit);
        }
        else
        {
            bulletHasBeenShot = this.gun.Shoot(this.bulletBarrelLocation.position, crosshairWorldTargetPosition, false);
        }

        if (bulletHasBeenShot)
        {
            this.PlayMuzzleFlash();
            this.muzzleFlashParticleSystem.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        }
    }

    private void PlayMuzzleFlash()
    {
        //Play the particle system attached to the muzzle flash particle system object
        //Also play the particle system attached to any of its children
        this.muzzleFlashParticleSystem.Play();
        for (int i = 0; i < this.muzzleFlashParticleSystem.transform.childCount; i++)
        {
            if (this.muzzleFlashParticleSystem.TryGetComponent(out ParticleSystem particle))
            {
                particle.Play();
            }
        }
    }
}
