using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCreeper_NormalType : Enemy
{
    public Transform groundCheckerTransform;
    public float flyHeight = 1;
   
    public Transform RayOriginTransform;
    public float RayLength;



    private float groundHeight;
    private RaycastHit hit;
    private Ray ray;
   [SerializeField] private Renderer matRenderer;

    

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
        //派生移动

        if (groundHeight != 0f)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, flyHeight + groundHeight, transform.position.z), 1 * Time.deltaTime);
        AttackCheck();
        GroundCheck();

    }
    /// <summary>
    /// 转换成guard形态
    /// </summary>
    [ContextMenu("Test")]
    public void ChangeType()
    {
        Defence = 15;
        MaxHealth = 20;
        Health = 20;
        matRenderer.material.SetFloat("_addFloat", 2);
    }
    private void AttackCheck()
    {
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 1, Vector3.down, RayLength, 9);
        //Debug.DrawRay(RayOriginTransform.position, Vector3.down);
        if (target != null )
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


    private void FixedUpdate()
    {

        if (CantMove || IsVertigo)
        {
            return;
        }
        rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
        MoveToTheNearestRow();
    }
    public override void Death()
    {
        base.Death();
        SoundSystem.Instance.PlayRandom2Dsound(ResourceSystem.Instance.GetEnemy(type).DeathSounds);
        OnDisAppear();
    }
    public void Attack()
    {
        if (target != null)
        {
            target.Hurt(FinalDamage);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
        }
       
    }


}
