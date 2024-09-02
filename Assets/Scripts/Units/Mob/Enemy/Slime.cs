using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public FunctionalBlock targetBlock;
    public Transform RayOriginTransform;
    public float RayLength;
    public float Gravity;
    public AudioClip[] slimeSounds;
    private bool _CanJump;
    public int Type;
    protected override void Start()
    {
        base.Start();
        Walk();

    }
    private void FixedUpdate()
    {
        rb.velocity -= new Vector3(0, Gravity, 0);
        if (IsVertigo == false && CantMove == false)
        {
            MoveToTheNearestRow();
            if (_CanJump == false)
            {
                rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, rb.velocity.z);
            }
        }

    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        CheckGround();
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, new Vector3(-1, 0, 0), RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, new Vector3(-1, 0, 0), RayLength, 3);

        if (transform.position.z < -10)
        {
            Hurt(1);
        }

        if (target != null || (targetBlock != null))
        {
            StartAttack();
        }
        else
        {
            EndAttack();
        }
    }
    public override void OnDisAppear()
    {
        
        if (EnemyManager.Instance != null)
        {
            float posZ = FindTheNeareastRowPosz();

            float minl = EnemyManager.Instance.GetMinLinePosZ();
            float maxl = EnemyManager.Instance.GetMaxLinePosZ();
            if (posZ + 1.9f <= maxl)
            {
                SummonChild(posZ + 2);
            }
            else
            {
                SummonChild(posZ);
            }
            if (posZ - 1.9f >= minl)
            {
                SummonChild(posZ - 2);
            }
            else
            {
                SummonChild(posZ);
            }

        }
        base.OnDisAppear();
    }
    private void SummonChild(float posz)
    {
        switch (Type)
        {
            case 0:
                EnemyManager.Instance.CreateEnemy(EnemyType.Slime_Middle, new Vector3(transform.position.x, transform.position.y, posz));
                break;
            case 1:
                EnemyManager.Instance.CreateEnemy(EnemyType.Slime_Small ,new Vector3(transform.position.x, transform.position.y, posz));
                break;
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
    public void JumpForward()
    {
        if (_CanJump == true)
        {
            rb.velocity = new Vector3(-FinalSpeed, JumpForce, 0);
        }

    }

    public void JumpSound()
    {
        PlayRandomSounds(slimeSounds);
    }
    private void CheckGround()//检测是否落地，约束跳跃为1次
    {
        float Height = 0.7f;
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Height, 1 << 3))
        {
            _CanJump = true;
        }
        else
        {
            _CanJump = false;
        }
    }

}
