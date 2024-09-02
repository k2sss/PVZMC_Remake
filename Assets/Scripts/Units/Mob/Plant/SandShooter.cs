using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandShooter : Plants
{
    public Transform CheckPoint;
    public Transform ShootPoint;
    public GameObject bullet;
    public AudioClip ShootSound;
    protected override void Update()
    {
        base.Update();
        RayCheck();
        HurtRig();
    }

    private void RayCheck()
    {

        if (MyPhysics.BoxRayCheck<Mob>(CheckPoint.position, 0.5f, new Vector3(1,0,0), 5, 8) != null)
        {
            _animator.SetBool("Attack", true);
            IsAttacking = true;
        }
        else
        {
            IsAttacking = false;
            _animator.SetBool("Attack", false);
        }
    }
    public void Shoot()
    {
        PlayOneShot(ShootSound,0.4f);
        for (int i = 0; i < 10; i++)
        {
            MonoController.Instance.Invoke(Random.Range(0f, 0.2f), OneShot);
        }
       

    }

    private void OneShot()
    {
        Bullet sandBullet = ObjectPool.Instance.GetObject(bullet).GetComponent<Bullet>();
        sandBullet.transform.position = ShootPoint.transform.position;
        sandBullet.init(new Vector3(1, Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f)), FinalDamage, Random.Range(6,9f));
        
    }
    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
    


    
}
