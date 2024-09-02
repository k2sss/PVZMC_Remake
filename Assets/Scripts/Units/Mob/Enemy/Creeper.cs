using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creeper : Enemy,Ipurefiable
{
    public Transform RayOriginTransform;
    public float RayLength;
    public FunctionalBlock targetBlock;
    private float explodeTimer;
    public GameObject explodeObject;
    public AudioClip igniteSound;
    private float speedUpTime;
    private bool IsSpeedUp;
    public float explode1;
    public float explode2;
    public bool isBlood;
    public Material Greenmat;
    protected override void Start()
    {
        base.Start();
        Walk();
        InvokeRepeating("CheckGround", 0, 0.3f);
        if(isBlood)GenerateHpBar();
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
        HurtRig();
      
        if(!isBlood)
        speedUpTime += Time.deltaTime;
        if (speedUpTime > 15&&IsSpeedUp == false)
        {
            IsSpeedUp = true;
            SetNowSpeed(2f);
            SetAnimatorSpeed(3f);
        }

        if (IsAttacking == true&&IsDead == false)
        {
            explodeTimer += Time.deltaTime;
            ChangeMaterialColor(Color.white * (1+explode1 * Mathf.Abs(Mathf.Sin(explodeTimer * explode2))));
        }

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
    private void CheckGround()
    {
        if (isBlood)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, Vector3.down);
     
            if (Physics.Raycast(ray, out RaycastHit hit, 2, 1<<6))
            {
                if (hit.collider.GetComponent<Grid>().gridType == GridType.blood)
                {
                    if (IsSpeedUp == false)
                    {
                        IsSpeedUp = true;
                        SetNowSpeed(2f);
                        SetAnimatorSpeed(2f);
                    }
                }
                else
                {
                    if (IsSpeedUp == true)
                    {
                        IsSpeedUp = false;
                        SetNowSpeed(-2f);
                        SetAnimatorSpeed(-2f);
                    }
                }
            }
            
        }

    }
    public override void StartAttack()
    {  
        if(IsAttacking == false)
        PlayOneShot(igniteSound);
        base.StartAttack();
      
    }
    public override void EndAttack()
    {
        
        if (IsAttacking == true)
        {
        ChangeMaterialColor(Color.white);
        explodeTimer = 0;
        }
        base.EndAttack();
        
    }
    public void Explode()
    {  
      

        CameraAction.Instance.StartShake();
        GameObject a = ObjectPool.Instance.GetObject(explodeObject);
        a.transform.position = transform.position;
        float f = FinalDamage;
        if(!isBlood)
        a.GetComponent<ExplodeBox>().InitDamage(f * 7, f * 7, f * 100, f*100);
        else
        {
          a.GetComponent<BlockBox>().InitDamage(f,f,3);
            MonoController.Instance.Invoke(0.1f, WorldManager.UpdateWorldInfo);
        }
        onMobDisapear?.Invoke();
        Destroy(gameObject);
        
      
    }
    public override void Death()
    {
        base.Death();
        if (Random.Range(0, 100) < 30)
        {
            Explode();
        }
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage, penetration, Dtype);
        if (isBlood) ShowDamage(Damage);
    }
    void Ipurefiable.OnPurify()
    {
        if(isBlood == true)
        {
            isBlood = false;
            ChangeMaterial(Greenmat);
        }
    }

    void Ipurefiable.OnEndPurify()
    {
       
    }
}
