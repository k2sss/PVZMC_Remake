using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Licker : Enemy,Ipurefiable
{
    public Transform RayOriginTransform;
    public float RayLength;
    [HideInInspector] public FunctionalBlock targetBlock;
    [HideInInspector] public Mob TongueTarget;
    private float tongueAttackTimer = 5;
    private bool isAbleToTogueAttack = true;
    private bool isTongueAttacking;
    protected override void Start()
    {
        base.Start();
        GenerateHpBar();
        Walk();
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();


        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, new Vector3(-1, 0, 0), RayLength, 9);
        TongueTarget = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, new Vector3(-1, 0, 0), 6, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, new Vector3(-1, 0, 0), RayLength, 3);

        if (target != null || (targetBlock != null))
        {
            StartAttack();
        }
        else
        {
            EndAttack();
        }
        if (!isAbleToTogueAttack) return;
        tongueAttackTimer += Time.deltaTime;
        if (tongueAttackTimer > 10 && TongueTarget != null && target == null && targetBlock == null)
        {
                _animator.SetBool("IsAttack2", true);
                isTongueAttacking = true;

        }
        else
        {
               _animator.SetBool("IsAttack2", false);
               isTongueAttacking = false;
        }


    }
    public void TongueAttack()
    {
        //Plants t_plant = TongueTarget as Plants;
        //t_plant.targetGrid.RemovePlant();
        MonoController.Instance.Invoke(0.4f,()=> tongueAttackTimer = 0);
        TongueTarget.Hurt(2);
        Vertigo(0.5f);
        AddForce(new Vector3(-6, 2, 0));


    }
    public void Attack()
    {
        if (target != null)
        {
            target.Hurt(FinalDamage);
            target.AddBuff(BuffType.Poison, 5);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
        }
        else if (targetBlock != null)
        {
            targetBlock.CauseDamage(FinalDamage, BlockStrengthType.normal, 0, 1, 0.3f);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds, 0.3f);
        }
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage, penetration, Dtype);
        ShowDamage(Damage);
    }
    private void FixedUpdate()
    {

        if (CantMove == false && IsVertigo == false && isTongueAttacking == false)
        {
            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
    }

    void Ipurefiable.OnPurify()
    {
        SetNowSpeed(-0.4f);
        isPurified = true;
        isAbleToTogueAttack = false;
    }

    void Ipurefiable.OnEndPurify()
    {
        SetNowSpeed(0.4f);
        isPurified = false;
        isAbleToTogueAttack = true;
    }
}
