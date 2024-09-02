using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonKnight : Enemy,Ipurefiable
{
    public AttackBox box;
    public Transform RayOriginTransform;
    public float RayLength = 1;
    private FunctionalBlock targetBlock;
    private bool isCrazy;
    public void OnEndPurify()
    {
        isPurified = false;
        Defence = 10;
    }

    public void OnPurify()
    {
        isPurified = true;
        Defence = 0;
    }
    protected override void Start()
    {
        
        GenerateHpBar();
        base.Start();
        Walk();
        InvokeRepeating("CheckGround", 0, 0.5f);
    }

    protected override void Update()
    {
        base.Update();
        HurtRig();
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, Vector3.left, RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, Vector3.left, RayLength, 3);

        if (target != null || (targetBlock != null))
        {
            StartAttack();
        }
        else
        {
            EndAttack();
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
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage, penetration, Dtype);
        ShowDamage(Damage);
    }
    public void Attack(float damage)
    {
        box.Damage = damage * FinalDamage;
        box.PlayerDamage = damage * FinalDamage;
        box.gameObject.SetActive(true);
        MonoController.Instance.Invoke(0.1f, () => box.gameObject.SetActive(false));
        PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
    }
    private void CheckGround()
    {
        
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 2, 1 << 6))
            {
                if (hit.collider.GetComponent<Grid>().gridType == GridType.blood)
                {
                    if (isCrazy == false)
                    {
                        isCrazy = true;
                         _animator.SetBool("IsRun", true);
                       SetNowSpeed(1f);
                      
                       
                    }
                }
                else
                {
                    if (isCrazy == true)
                    {
                        isCrazy = false;
                       _animator.SetBool("IsRun", false);
                        MonoController.Instance.Invoke(0.4f, () => SetNowSpeed(-1f));

                }
                }
            }

        

    }
}
