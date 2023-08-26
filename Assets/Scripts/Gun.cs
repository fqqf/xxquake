using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    InputAction shoot;

    public Transform fpsCam;
    public float impactForce = 150;

    public float range = 100;

    public int fireRate = 10;

    float nextTimeToFire = 0;

    public ParticleSystem muzzleFlush;
    public GameObject impactEffect;
    public Light muzzleFlushLight;
    // Start is called before the first frame update
    void Start()
    {
        shoot = new InputAction("Shoot", binding: "<mouse>/leftButton");
        shoot.AddBinding("<Gamepad>/x");
        shoot.Enable();
        Color color = muzzleFlushLight.color;
    }

    // Update is called once per frame
    
    
    void Update()
    {
        bool isShooting = shoot.ReadValue<float>() == 1;

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
    }
    
    private void Fire()
    {
        RaycastHit hit;
        AudioManager.instance.Play("Shoot");
        muzzleFlush.Play();
        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                
                hit.rigidbody.AddForce(-hit.normal*impactForce);

            }
            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject impact = Instantiate(impactEffect, hit.point, impactRotation);
            Destroy(impact, 5);
        }
    }
}
