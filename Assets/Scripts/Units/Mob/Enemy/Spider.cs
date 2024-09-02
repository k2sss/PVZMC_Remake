using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    public Transform RayOriginTransform;
    public float RayLength;
    public FunctionalBlock targetBlock;
    protected override void Start()
    {
        base.Start();
        Walk();
        SoundSystem.Instance.RigistEnemyIdleSounds(this, 5, 12);
    }
    protected override void Update()
    {
        base.Update();
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, new Vector3(-1, 0, 0), RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, new Vector3(-1, 0, 0), RayLength, 3);
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
    public virtual void Attack()
    {
        if (target != null)
        {
            target.Hurt(FinalDamage);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
        }
        else if (targetBlock != null)
        {
            targetBlock.CauseDamage(FinalDamage, BlockStrengthType.normal, 0, 1, 0.3f);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds, 0.3f);
        }

    }
}
