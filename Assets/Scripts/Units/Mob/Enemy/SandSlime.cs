using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSlime : Enemy
{
    public FunctionalBlock targetBlock;
    public Transform RayOriginTransform;
    public float RayLength;
    public float Gravity;
    public AudioClip[] slimeSounds;
    private bool _CanJump;
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
        }
       
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        CheckGround();
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, -transform.right, RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, -transform.right, RayLength, 3);

        float size = Mathf.Clamp(Health / MaxHealth,0.4f,1) ;
        transform.localScale = new Vector3(size, size, size);

        if (target != null || (targetBlock != null))
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
        float Height = 0.2f;
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
