using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpion : Enemy
{
    public Transform RayOriginTransform;
    public float RayLength;
    public FunctionalBlock targetBlock;
    public float shootTimer;
    public GameObject bullet;
    public Transform bulletGeneratePos;
    public AudioClip throwClip;
    private float timeCell = 5;
    
    protected override void Start()
    {
        base.Start();
        Walk();
        SoundSystem.Instance.RigistEnemyIdleSounds(this, 5, 12);
        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.ScorpionAttackSpeedUp == true)
        {
            timeCell = 2;
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

    // Update is called once per frame
    protected override void Update()
    {
        shootTimer += Time.deltaTime;
        base.Update();
        base.HurtRig();//控制受伤 骨骼限制
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, transform.forward, RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1, transform.forward, RayLength, 3);
        if(shootTimer > timeCell && IsAttacking == false)
        {
            shootTimer = 0;
            StartShoot();
            MonoController.Instance.Invoke(0.7f, EndShoot);
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

    public void Attack()
    {
        if (target != null)
        {
            target.Hurt(FinalDamage);
            target.AddBuff(BuffType.Poison, 2);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
        }
        else if (targetBlock != null)
        {

            targetBlock.CauseDamage(FinalDamage, BlockStrengthType.normal, 0, 1, 0.3f);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds, 0.3f);
        }

    }
    public void StartShoot()
    {
         _animator.SetBool("IsShoot", true);
         CantMove = true;
    }
    public void EndShoot()
    {
        _animator.SetBool("IsShoot", false);
        CantMove = false; 
    }
    public void Shoot()//发射一颗子弹，由动画事件调用
    {
        GameObject newBullet = ObjectPool.Instance.GetObject(bullet);
        newBullet.transform.position = bulletGeneratePos.position;
        Bullet bulletSet = newBullet.GetComponent<Bullet>();
        bulletSet.init(new Vector3(-1, 0, 0),FinalDamage, 10, true);
        _audioSource.PlayOneShot(throwClip);
    }
    public override void Death()
    {
        base.Death();
        Stop();
    }
}
