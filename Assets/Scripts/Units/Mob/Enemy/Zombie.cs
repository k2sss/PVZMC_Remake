using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy,Ipurefiable
{
    public GameObject FallenHead;
    public GameObject FallenHand;
    public float WhenToFallHand;
    public float WhenToFallHead;
    private bool isFallHand;
    private bool isFallHead;
    public Vector3 HurtDir;
    public GameObject[] bloods;
    public Transform RayOriginTransform;
    public float RayLength;
    public AudioClip[] ZombieFallDownSounds;
    public FunctionalBlock targetBlock;
    protected override void Start()
    {
        base.Start();
        Walk();
        if (type == EnemyType.zombie_terraria)
        {
            GenerateHpBar();
        }
        SoundSystem.Instance.RigistEnemyIdleSounds(this, 5, 12);

    }
    private void FixedUpdate()
    {
        
        if (CantMove == false && IsVertigo == false)
        {   rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
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
       
        if (target != null||(targetBlock!=null))
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
        if (type == EnemyType.zombie_Blood)// && )
        {
            if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.BloodZombieMoveFastWhenHurt == true)
                AddBuff(BuffType.SpeedUp_VeryFast, 1f);
            else
                AddBuff(BuffType.SpeedUp_VeryFast, 0.3f);
        }
        else if (type == EnemyType.zombie_terraria)
        {
            ShowDamage(Damage);
        }

        if (Health <= WhenToFallHand&&isFallHand == false)
        {
            isFallHand = true;
            if (Random.Range(0, 100) < 60)
            {
            FallBodyPart(FallenHand,new Vector3(3,2,0));
            bloods[0].SetActive(true);
            }
        } 
        if (Health <= WhenToFallHead&&isFallHead == false)
        {
            isFallHead = true;
            FallBodyPart(FallenHead, new Vector3(3, 2, 0));
            bloods[1].SetActive(true);
        }
        
    }
    private void FallBodyPart(GameObject part,Vector3 direction)//掉落身体的某个部位
    {
        part.transform.parent = null;
        Renderer render = part.GetComponent<Renderer>();
        render.material.color = Color.white; 
        renderers.Remove(render);
        Rigidbody hrb = part.AddComponent<Rigidbody>();
        hrb.velocity = direction;
        hrb.AddTorque(direction*Random.Range(0,100));
        part.AddComponent<BoxCollider>();
        part.AddComponent<TimeDestroy>().SetTimer(3);
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
            targetBlock.CauseDamage(FinalDamage,BlockStrengthType.normal,0,1,0.3f);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds,0.3f);
        }

    }

    public void PlayFallDownSound()
    {
        CantMove = true;
        PlayRandomSounds(ZombieFallDownSounds);
    }

    void Ipurefiable.OnPurify()
    { 
        if (type == EnemyType.zombie_terraria)
        {
            isPurified = true;
            Hurt(10);
        }
    }

    void Ipurefiable.OnEndPurify()
    {
        isPurified = false;
    }
}
