using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryShooter : Plants
{
    public GameObject bullet;
    public Transform ShootStartTransform;
    public AudioClip[] shootSounds;
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

        if (MyPhysics.BoxRayCheck<Mob>(ShootStartTransform.position, 0.5f, new Vector3(1, 0, 0), 40, 8) != null)
        {
            _animator.SetBool("Attack", true);
            IsAttacking = true;
        }
        else
        {
            _animator.SetBool("Attack", false);
            IsAttacking = false;
        }
    }
    public void ShootBullet()
    {
        PlayRandomSounds(shootSounds);
        Shoot();
    }
    private void Shoot()
    {
        GameObject bullet = ObjectPool.Instance.GetObject(this.bullet);
        bullet.transform.position = ShootStartTransform.position;
        PeaBullet PeaBullet = bullet.GetComponent<PeaBullet>();
        PeaBullet.init(new Vector3(1,0,0),FinalDamage, 15);

    }
}
