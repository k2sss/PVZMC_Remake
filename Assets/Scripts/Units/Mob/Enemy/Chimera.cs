using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : Enemy,Ipurefiable
{
    public Transform groundCheckerTransform;
    public float flyHeight = 1;
    public Vector3 targetDir;
    public Transform RayOriginTransform;
    public float RayLength;
    public float searchPlayerRange = 3;//仅水平范围
    public GameObject body;
    private float groundHeight;
    private RaycastHit hit;
    private Ray ray;

    public FunctionalBlock targetBlock;

    protected override void Start()
    {
        base.Start();
        Walk();
       
        GenerateHpBar();
        ray = new Ray();
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        //派生移动
       
        if(groundHeight != 0f)
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, flyHeight + groundHeight, transform.position.z), 1 * Time.deltaTime);
        AttackCheck();
        GroundCheck();

    }
    private void AttackCheck()
    {
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, -transform.right, RayLength, 9);
        targetBlock = MyPhysics.BoxRayCheck<FunctionalBlock>(RayOriginTransform.position, 1.5f, -transform.right, RayLength, 3);

        if (target != null || (targetBlock != null))
        {
            StartAttack();
        }
        else
        {
            EndAttack();
        }

    }
    private void GroundCheck()
    {

        ray.direction = Vector3.down;
        ray.origin = groundCheckerTransform.position;

        if (Physics.Raycast(ray, out hit, 40, 1 << 3))
        {
            groundHeight = hit.point.y;
        }
        else
        {
            groundHeight = 0;
        }
    }
 

    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage, penetration, Dtype);
        ShowDamage(Damage);
    }

    private void FixedUpdate()
    {

        if (CantMove == false && IsVertigo == false)
        {


           
                rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
                MoveToTheNearestRow();

                transform.right = Vector3.Slerp(transform.right, new Vector3(1, 0, 0), 1 * Time.deltaTime);
           
            

        }
    }
    public override void Death()
    {
        base.Death();
        DeadSoon(body);
    }
    public void Attack()
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

    void Ipurefiable.OnPurify()
    {
        SetNowSpeed(-0.5f);
        isPurified = true;
    }

    void Ipurefiable.OnEndPurify()
    {
        SetNowSpeed(0.5f);
        isPurified = false;
    }
}
