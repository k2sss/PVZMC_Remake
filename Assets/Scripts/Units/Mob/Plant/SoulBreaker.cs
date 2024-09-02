using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBreaker : Plants
{
    private Vector3 shootPos = new Vector3(0.3f, 1, 0);
    public GameObject bullet;

    protected override void Update()
    {
        base.Update();
        HurtRig();
        RayCheck();
    }
    private void RayCheck()
    {

        if (MyPhysics.BoxRayCheck<Mob>(transform.position + shootPos, 0.5f,Vector3.right,40,8)!=null)
        {
            _animator.SetBool("IsAttack", true);
            IsAttacking = true;
        }
        else
        {
            _animator.SetBool("IsAttack", false);
            IsAttacking = false;
        }
    }
    public void Attack()
    {
        GameObject bullet = ObjectPool.Instance.GetObject(this.bullet);
        bullet.transform.position = transform.position + shootPos;
        PeaBullet PeaBullet = bullet.GetComponent<PeaBullet>();
        PeaBullet.init(transform.forward, FinalDamage, 10);


    }
}
