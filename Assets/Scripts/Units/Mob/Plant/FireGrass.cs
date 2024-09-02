using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrass : Plants
{
    public Transform ShootStartTransform;
    public Transform ShootPoint;
    public GameObject bullet;
    private AudioClip[] clips;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        clips = SoundSystem.Instance.GetAudioClips("Ignite");
    }
    private void RayCheck()
    {

        if (MyPhysics.BoxRayCheck<Mob>(ShootStartTransform.position, 4f, new Vector3(1,0,0), 5, 8) != null)
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
    public void Shoot()
    {
      GameObject b =  ObjectPool.Instance.GetObject(bullet);
      b.transform.position = ShootPoint.position;
        b.GetComponent<Bullet>().init(new Vector3(1, 0, 0),FinalDamage, 5);
        PlayRandomSounds(clips);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        HurtRig();
        RayCheck();
    }
}
