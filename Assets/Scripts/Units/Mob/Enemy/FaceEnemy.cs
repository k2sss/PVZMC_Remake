using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnemy : Enemy,Ipurefiable
{
    public Transform RayOriginTransform;
    public float RayLength;
    public GameObject departBody;
    private float HurtTimer;
    public FunctionalBlock targetBlock { get; private set; }
    protected override void Start()
    {
        base.Start();
        GenerateHpBar(); 
        Walk();
        SoundSystem.Instance.RigistEnemyIdleSounds(this, 7, 20);
    }
    private void FixedUpdate()
    {

        if (CantMove == false && IsVertigo == false)
        {
            if(HurtTimer <0)
            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            else
            rb.velocity = new Vector3(-FinalSpeed * 0.3f, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
    }

    protected override void Update()
    {
        HurtTimer -= Time.deltaTime;
        base.Update();
       
        HurtRig();
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, new Vector3(-1,0,0), RayLength, 9);
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
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {

        base.Hurt(Damage, penetration, Dtype);
        ShowDamage(Damage);
        HurtTimer = 0.3f;
    }
    public override void Death()
    {
        base.Death();
        DeadSoon(departBody);
    }

    void Ipurefiable.OnPurify()
    {
        SetNowSpeed(-0.3f);
        isPurified = true;
    }

    void Ipurefiable.OnEndPurify()
    {
        SetNowSpeed(0.3f);
        isPurified = false;
    }
}
