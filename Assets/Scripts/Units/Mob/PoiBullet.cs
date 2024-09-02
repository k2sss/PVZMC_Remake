using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiBullet : Bullet
{
    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = direction.normalized * BulletSpeed;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        OnHit(other);
        if(other.CompareTag("Player")|| other.CompareTag("Plants") )
        other.GetComponent<Mob>().AddBuff(BuffType.Poison, 3);
    }
}
