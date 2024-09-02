using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderMan : Enemy
{
    private bool IsAngry;
    public AudioClip stareAudioClip;
    public FunctionalBlock targetBlock;
    public Transform RayOriginTransform;
    public float RayLength;
    public ParticleSystem particle;
    private float transferTimer;
    public AudioClip[] portalAudioClips;
    private float timer2;

    protected override void Start()
    {
        base.Start();
        Walk();
        SoundSystem.Instance.RigistEnemyIdleSounds(this, 5, 12); 
    }
    private void FixedUpdate()
    {
        if (CantMove == false && IsVertigo == false)
        {
            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {

        base.Hurt(Damage, penetration, Dtype);
        BecomeAngry();
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        if (transferTimer > 0)
        {
            ChangeMaterialColor(Color.white * (1 + transferTimer * 20));
            transferTimer -= Time.deltaTime;
        }
        timer2 += Time.deltaTime;
        if (timer2 > 30)
        {
            timer2 = 0;
            ToOtherLine();
        }
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, transform.forward, RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, transform.forward, RayLength, 3);

        if (target != null || (targetBlock != null))
        {
            StartAttack();
        }
        else
        {
            EndAttack();
        }
    }
    public void BecomeAngry()
    {
        if (IsAngry == false)
        {
            _animator.SetBool("IsAngry", true);
            IsAngry = true;
            PlayOneShot(stareAudioClip);
            SetNowSpeed(0.8f);
            SetAnimatorSpeed(0.8f);
            SetAttackSpeed(0.4f);
        }
        
    }
    private void Attack()
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
    public override void Death()
    {
        _animator.speed = 1;
        base.Death();
    }
    [ContextMenu("Transfer")]
    public void ToOtherLine()//转到另一行
    {
        particle.Emit(20);
        if (transferTimer < 0.1f)
        {
            PlayRandomSounds(portalAudioClips);
        }
       
        transferTimer = 0.3f;
        
        transform.position = new Vector3(transform.position.x, transform.position.y, FindTheRandomRowPosZ());
       
    }
  
}
