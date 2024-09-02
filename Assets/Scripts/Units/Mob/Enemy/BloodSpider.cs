using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BloodSpider : Enemy,Ipurefiable
{
    public Rig rig;
    private float z1, z2;
    public Transform[] rayOrigin;
    private Vector3 targetUp;
    private Vector3 targetDir;
    private Quaternion targetRot;
    
    private float timer = 1;
    private bool IsTouchGround = true;
    private Quaternion firstPos;
    private Vector3 targetPos;
    private float transTimer;
    private float backWayTimer;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        Walk();
        GenerateHpBar();
        z1 = GridManager.Instance.GetMinZ() + 1f;
        z2 = GridManager.Instance.GetMaxZ() - 1f;
        
        targetDir = -transform.forward;
        if(Random.Range(0,100)<50)
        targetDir.z = -targetDir.z;
        targetRot= transform.localRotation;
        firstPos = transform.localRotation;
        targetPos = transform.position;
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        ChangeTargetDir();
        


        //射线
        Ray rayforward = new Ray(rayOrigin[0].position, -transform.forward);
        Ray raydown = new Ray(rayOrigin[0].position, -transform.up);
        Ray rayback = new Ray(rayOrigin[1].position, transform.forward);
        timer += Time.deltaTime;
        transTimer -= Time.deltaTime;
        //如果方向反了并且时间超了，就转身回去
        if (targetDir.x > 0.3f)
        {
            backWayTimer += Time.deltaTime;
        }
        else
        {
            backWayTimer = 0;
        }

        //Debug.DrawRay(rayOrigin[0].position, -transform.forward * 0.6f);
        //Debug.DrawRay(rayOrigin[0].position, -transform.up);
        //Debug.DrawRay(rayOrigin[1].position, transform.forward * 2);

        //位置插值
        if (transTimer > 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 1 * Time.deltaTime);
        }
        //检测墙壁并攀爬
        if (timer > 0.5f)
        {
            if (Physics.Raycast(rayforward, out RaycastHit hit, 1f, 1 << 3))//前方
            {
                targetUp = hit.normal;

                var q = Quaternion.FromToRotation(transform.up, hit.normal);

                targetPos = hit.point + targetDir ;
                transform.Rotate(q.eulerAngles,Space.World);
                targetDir = -transform.forward;
                //Debug.Log(targetDir);
                transform.Rotate(-q.eulerAngles, Space.World);
                transTimer = 1;
                
            }
            else
            {
                if (Physics.Raycast(raydown, out RaycastHit hit2, 1f, 1 << 3))
                {
                    IsTouchGround = true;

                }
                else//没检测到
                {
                    
                    if (Physics.Raycast(rayback, out RaycastHit hit3, 2, 1 << 3))
                    {
                        targetUp = hit3.normal;
                        var e = Quaternion.FromToRotation(transform.up, hit3.normal);
                        transform.Rotate(e.eulerAngles, Space.World);
                        targetDir = -transform.forward;
                        transform.Rotate(-e.eulerAngles, Space.World);
                        IsTouchGround = true;
                        targetPos = hit3.point;
                        transTimer = 1;
                    }
                    else
                    {
                        IsTouchGround = false;
                    }

                }
            }
            timer = 0;
        }
        if (backWayTimer > 6)
        {
            targetDir.x = -targetDir.x;
            backWayTimer = 0;
        }
        //旋转插值
        var rot = Quaternion.LookRotation(-targetDir, targetUp);
        targetRot = rot;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, 5 * Time.deltaTime);

        //攻击逻辑
        target = MyPhysics.BoxRayCheck<Mob>(rayOrigin[2].position, 1, -transform.forward, 1, 9);
        //Debug.DrawRay(rayOrigin[2].position, -transform.forward);
        if (target != null)
        {
            //Debug.Log(target.name);
            StartAttack();
        }
        else
        {
            EndAttack();
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
            if(IsTouchGround)
            rb.velocity = FinalSpeed * targetDir.normalized;
        }
    }
    public override void Death()
    {
        base.Death();
        rig.weight = 0;
        Stop();
    }


    public void Attack()
    {
        if (target != null)
        {
            target.Hurt(FinalDamage);
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
        }
    }
    private void ChangeTargetDir()
    {
        if (targetDir.z > 0 && transform.position.z > z2)
            targetDir.z = -targetDir.z;
        else if (targetDir.z < 0 && transform.position.z < z1)
        {
            targetDir.z = -targetDir.z;
        }
    }

    void Ipurefiable.OnPurify()
    {
        SetNowDamage(-0.5f);
        isPurified = true;
    }

    void Ipurefiable.OnEndPurify()
    {
        SetNowDamage(0.5f);
        isPurified = false;
    }
}
