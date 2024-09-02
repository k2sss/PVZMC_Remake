using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow :Bullet
{
    
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        rb.velocity = direction * BulletSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        OnHit(other);
    }
}
