using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public InputAction shoot;

    public Transform fpsCam;
    public float impactForce = 150;

    public float range = 100;

    public int fireRate = 10;

    float nextTimeToFire = 0;
    
    public ParticleSystem muzzleFlush;
    public GameObject impactEffect;
    public Light muzzleFlushLight;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        shoot = new InputAction("Shoot", binding: "<mouse>/leftButton");
        shoot.AddBinding("<Gamepad>/x");
        shoot.Enable();
        Color color = muzzleFlushLight.color;
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame

    public float reloadTime = 2f;
    public bool isReloading;

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool(("is_reloading"), false);
    }

    void Update()
    {
        bool isShooting = shoot.ReadValue<float>() == 1;

        if (currentAmmo == 0 && magazineSize == 0)
        {
            animator.SetBool("is_shooting",false);
            return;
        }

        if (isReloading) return;
        
        animator.SetBool("is_shooting",isShooting);
        if (muzzleFlush.isPlaying) muzzleFlushLight.intensity = 600;
        else
        {
            muzzleFlushLight.intensity = 0;
        }

        if (isShooting && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }

        if (currentAmmo == 0 && magazineSize>0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetBool("is_reloading", isReloading);
        yield return new WaitForSeconds(reloadTime);
        if (magazineSize >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            magazineSize -= maxAmmo;
        }
        else
        {
            currentAmmo = magazineSize;
            magazineSize = 0;
        }

        isReloading = false;
        animator.SetBool("is_reloading", isReloading);
    }

    public int currentAmmo;
    public int maxAmmo = 10;
    public int magazineSize = 30;

    private void Fire()
    {

        RaycastHit hit;
        AudioManager.instance.Play("Shoot");
        muzzleFlush.Play();

        currentAmmo--;
        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                
                hit.rigidbody.AddForce(-hit.normal*impactForce);

            }
            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject impact = Instantiate(impactEffect, hit.point, impactRotation);
            impact.transform.parent = hit.transform;
            
            Destroy(impact, 5);
        }
    }
}
