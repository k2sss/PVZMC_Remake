using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieN : Enemy
{
    public GameObject FallenHead;
    public float WhenToFallHead;
    public Vector3 HurtDir;
    public GameObject blood;
    public Transform RayOriginTransform;
    public float RayLength;
    public AudioClip[] ZombieFallDownSounds;
    public FunctionalBlock targetBlock;
    public GameObject head;
    public Transform shootPoint;
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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        base.HurtRig();//控制受伤 骨骼限制
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

    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage);
        if (Health < MaxHealth/2)
        {
            _animator.SetBool("ThrowHead", true);
            Stop();
        }

    }
    public void ThrowHead()
    {
        FallenHead.gameObject.SetActive(false);
        GameObject h = Instantiate(head);
        h.transform.position = shootPoint.position;
        Vector3 direction = new Vector3(-3, 2, 0);
        Rigidbody hrb = h.GetComponent<Rigidbody>();
        hrb.velocity = direction;
        hrb.AddTorque(direction * Random.Range(0, 20));
    }
    public void bloodOn()
    {
        blood.SetActive(true);
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

    public void PlayFallDownSound()
    {
        CantMove = true;
        PlayRandomSounds(ZombieFallDownSounds);
    }
}
