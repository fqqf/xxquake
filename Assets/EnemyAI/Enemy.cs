using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject projectile;
    public Transform projectilePoint;
    
    void Start()
    {

    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        
        rb.AddForce(transform.forward*30f, ForceMode.Impulse);
        rb.AddForce(transform.up*7, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
