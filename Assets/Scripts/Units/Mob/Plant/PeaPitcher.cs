using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaPitcher : Plants
{
    public Mob target;
    public Transform ShootStartTransform;
    public GameObject bullet;
    public AudioClip throwSound;
    protected override void Update()
    {
        base.Update();
        HurtRig();
        target = MyPhysics.BoxRayCheck<Mob>(ShootStartTransform.position, 4f,20f, new Vector3(1, 0, 0), 12, 8);
        if ( target != null)
        {
            //Debug.Log(target.name);
            _animator.SetBool("IsAttack",true);
            IsAttacking = true;
        }
        else
        {
            _animator.SetBool("IsAttack", false);
            IsAttacking = false;
        }
    }
    private void Attack()
    {
        if (target != null)
        {
            
            GameObject b = ObjectPool.Instance.GetObject(bullet);
            b.transform.position = ShootStartTransform.position;
            b.GetComponent<PitcherBullet>().InitPictherBullet(target.transform.position + new Vector3(0,0.3f,0),FinalDamage);
            PlayOneShot(throwSound);
        }
    }
}
