using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunShooter : Plants
{
    public GameObject bullet;
    public Transform ShootStartTransform;
    public AudioClip[] shootSounds;
    public float RandomValue = 0.3f;
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

        if (MyPhysics.BoxRayCheck<Mob>(ShootStartTransform.position, 0.5f, new Vector3(1,0,0), 40, 8) != null)
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
        
        if (Random.Range(0, 100) <= 5)
        {
            _animator.SetBool("IsCrazyAttack", true);
            MonoController.Instance.Invoke(2,() => _animator.SetBool("IsCrazyAttack", false));
        }
        else
        {
            PlayRandomSounds(shootSounds);
            Shoot();
            Invoke("Shoot", 0.05f);
            Invoke("Shoot", 0.1f);
            Invoke("Shoot", 0.15f);
        }


    }
    private void CrazyShoot()
    {
        PlayRandomSounds(shootSounds);
        RandomShoot();
        Invoke("RandomShoot", 0.05f);
        Invoke("RandomShoot", 0.1f);
        Invoke("RandomShoot", 0.15f);
    }
    private void Shoot()
    {
        GameObject bullet = ObjectPool.Instance.GetObject(this.bullet);
        bullet.transform.position = ShootStartTransform.position;
        PeaBullet PeaBullet = bullet.GetComponent<PeaBullet>();
        PeaBullet.init(new Vector3(1, 0, 0), FinalDamage, 15);
    }
    private void RandomShoot()
    {
        GameObject bullet = ObjectPool.Instance.GetObject(this.bullet);
        bullet.transform.position = ShootStartTransform.position + new Vector3(0,Random.Range(-RandomValue, RandomValue), Random.Range(-RandomValue, RandomValue));
        PeaBullet PeaBullet = bullet.GetComponent<PeaBullet>();
        PeaBullet.init(new Vector3(1, 0, 0), FinalDamage, 15);
    }

}
