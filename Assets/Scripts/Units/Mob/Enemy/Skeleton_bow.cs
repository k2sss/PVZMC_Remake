using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_bow : Enemy
{
    public Transform RayOriginTransform;
    public float RayLength;
    public FunctionalBlock targetBlock;
    protected override void Start()
    {
        base.Start();
        Walk();
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, new Vector3(-1,0,0), RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, new Vector3(-1, 0, 0), RayLength, 3);
        if(GridManager.Instance != null &&GridManager.Instance.GetMaxX() + 2>= transform.position.x)
        if (target != null || (targetBlock != null))
        {
            StartAttack();
        }
        else
        {
            EndAttack();
        }
    }
    private void FixedUpdate()
    {
        if (CantMove == false && IsVertigo == false)
        {
            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
    }
    public void Shoot()
    {
        PlayOneShot(SoundSystem.Instance.GetAudioClips("Bow")[0]);
        GameObject arrowObj = ObjectPool.Instance.GetObject(ResourceSystem.Instance.GetBullet(BulletType.Normal_Arrow));
        arrowObj.transform.position = RayOriginTransform.position;
        Bullet bullet = arrowObj.GetComponent<Bullet>();

        bullet.init(new Vector3(-1, 0, 0), FinalDamage, 15,true);
        bullet.transform.forward = -new Vector3(1, 0, 0);
    }
}
