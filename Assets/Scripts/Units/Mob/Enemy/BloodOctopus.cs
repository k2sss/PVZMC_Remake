using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOctopus : Enemy, Ipurefiable
{
    public Transform groundCheckerTransform;
    public Transform RayOriginTransform;
    public float flyHeight = 1;
    public AudioClip ShootAudio;
    public GameObject bullet;
    private Vector3 preTargetPos;
    private float groundHeight;
    private Ray groundRay;
    protected override void Start()
    {
        base.Start();
        Walk();
        GenerateHpBar();
        InvokeRepeating("AttackCheck",0,0.5f);
        groundRay = new Ray(groundCheckerTransform.position, Vector3.down);
    }
    protected override void Update()
    {
        base.Update();

        HurtRig();
        GroundCheck();
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, flyHeight + groundHeight, transform.position.z), 1 * Time.deltaTime);
    }
  
    private void AttackCheck()
    {
        target = MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 2f, 20f, Vector3.left, 12, 9);
        if (target != null && isPurified == false)
        {
            StartAttack(); 
            preTargetPos = target.transform.position;
        }
        else
        {
            EndAttack();
        }

    }
    private void GroundCheck()
    {
        groundRay.direction = Vector3.down;
        groundRay.origin = groundCheckerTransform.position;
    
        if (Physics.Raycast(groundRay, out RaycastHit hit, 40, 1 << 3))
        {
            groundHeight = hit.point.y;
        }
        else
        {
            groundHeight = 0;
        }
    }
    private void Attack()
    {
        PlayAudioClip(ShootAudio);
        GameObject g = ObjectPool.Instance.GetObject(bullet);
        g.transform.position = RayOriginTransform.position;
        PitcherBullet b = g.GetComponent<PitcherBullet>();
        b.InitPictherBullet(preTargetPos, FinalDamage);
    }
    private void FixedUpdate()
    {
        
        if (CantMove == false && IsVertigo == false)
        {
            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
    }

    void Ipurefiable.OnPurify()
    {
        isPurified = true;
    }

    void Ipurefiable.OnEndPurify()
    {
        isPurified = false;
    }
}
