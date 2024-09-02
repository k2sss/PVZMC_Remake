using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public FunctionalBlock targetBlock;
    public Transform RayOriginTransform;
    public float RayLength;
    private bool IsHide = true;
    protected override void Start()
    {
        base.Start();
        Walk();
        
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, -transform.right, RayLength, 3);
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, -transform.right, RayLength, 9);
        if (targetBlock != null||(target != null&&IsHide == false))
        {
            StartAttack();
        }
        else
        {
            EndAttack();
        }
    }
    public virtual void Attack()
    {
        if (targetBlock != null)
        {
            targetBlock.CauseDamage(FinalDamage, BlockStrengthType.normal, 0, 1, 0.3f);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds, 0.3f);
        }
        if (target != null&&IsHide == false)
        {
            target.Hurt(FinalDamage);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
        }
    }
    private void SetHideState(bool hide)
    {
        IsHide = hide;
    }
    private void FixedUpdate()
    {
        if (CantMove == false && IsVertigo == false)
        {

            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
       
    }

}
