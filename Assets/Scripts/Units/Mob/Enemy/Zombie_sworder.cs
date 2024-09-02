using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_sworder : Enemy
{
    public GameObject FallenHead;
    public float WhenToFallHead;
    private bool isFallHead;
    public Vector3 HurtDir;
    public GameObject blood;
    public Transform RayOriginTransform;
    public float RayLength;
    public AudioClip[] ZombieFallDownSounds;
    public FunctionalBlock targetBlock;
    public bool IsCrazyMode;
    protected override void Start()
    {
        base.Start();
        Walk();
   

    }
    private void FixedUpdate()
    {
        if (CantMove == false && IsVertigo == false)
        {
        rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
           
    }
    protected override void Update()
    {
        base.Update();
        base.HurtRig();//控制受伤 骨骼限制
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, transform.forward, RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, transform.forward, RayLength, 3);
        if (IsCrazyMode == false&&Health < MaxHealth/4||(IsCrazyMode == false &&BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.SworderCrazyMode == true))//如果低于1/4血，开启狂暴状态
        {
            BaseSpeed = 2;
            BaseDamage = 15;
            FinalDamage = BaseDamage;
            SetNowSpeed(0.3f);
            _animator.SetBool("Phase2", true);
            IsCrazyMode = true;
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

    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage);
        if (Health <= WhenToFallHead && isFallHead == false)
        {
            isFallHead = true;
            FallBodyPart(FallenHead, new Vector3(3, 2, 0));
            blood.SetActive(true);
        }

    }
    private void FallBodyPart(GameObject part, Vector3 direction)//掉落身体的某个部位
    {
        part.transform.parent = null;
        Renderer render = part.GetComponent<Renderer>();
        render.material.color = Color.white;
        renderers.Remove(render);
        Rigidbody hrb = part.AddComponent<Rigidbody>();
        hrb.velocity = direction;
        hrb.AddTorque(direction * Random.Range(0, 100));
        part.AddComponent<BoxCollider>();
        part.AddComponent<TimeDestroy>().SetTimer(3);
    }
    public virtual void Attack1()
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
    public void PlayFallDownSound()
    {
        CantMove = true;
        PlayRandomSounds(ZombieFallDownSounds);
    }

}
