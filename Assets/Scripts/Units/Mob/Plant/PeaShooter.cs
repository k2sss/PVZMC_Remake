using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaShooter : Plants
{
    public GameObject bullet;
    public Transform ShootStartTransform;
    public AudioClip[] shootSounds;
    private int ShootNum;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        RayCheck();
        HurtRig();
    }

    private void RayCheck()
    {

        if (MyPhysics.BoxRayCheck<Mob>(ShootStartTransform.position,0.5f,transform.forward,40,8)!=null)
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
    public void ShootBullet()
    {
        PlayRandomSounds(shootSounds);
        ShootNum++;
        if (ShootNum >= 3)
        {
            ShootNum = 0;
            MonoController.Instance.Invoke(0.1f, Shoot);
        }
        Shoot();
    }
    private void Shoot()
    {
         GameObject bullet = ObjectPool.Instance.GetObject(this.bullet);
          bullet.transform.position = ShootStartTransform.position;
          PeaBullet PeaBullet = bullet.GetComponent<PeaBullet>();
          PeaBullet.init(transform.forward, FinalDamage, 15);
    }

}
