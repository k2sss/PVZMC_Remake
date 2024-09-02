using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoss : Enemy
{
    public AudioClip thump;
    public GameObject jumpParticle;
    public GameObject hitParticle;
    public AudioClip roar;
    private float RoarTimer;
    private float RoarRandomTime;
    private float prehealth;
    private float prehealthTimer;
    public Transform RayOriginTransform;
    public GameObject[] attackBoxes;
    private float attackTimer;
    private Vector3 TargetPos;
    public float FollowSpeed = 3;
    public float GravityAdder = 3;
    private bool IsJumpDown;
    private int JumpCount;//已经跳跃的次数
    public GameObject targetCross;
    private Vector3 OriginPos;
    public GameObject DirtBullet;
    public Transform handTransform;
    public GameObject ZombieNHead;
    private float ThrowHeadTimer;
    protected override void Start()
    {
        base.Start();
        //1代表怒吼，2代表防御，3代表捶地
        //怒吼 ==> 僵尸的进攻欲望增强
        //防御 ==> 会加血并且防御力提升
        //
        _animator.SetInteger("Attack", 1);

        RoarRandomTime = Random.Range(25, 40);
        prehealth = Health;
        TargetPos = transform.position;
        OriginPos = new Vector3(transform.position.x,2,transform.position.z);
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        if (TargetPos != Vector3.zero)
        {
            Vector3 newV = new Vector3(TargetPos.x, transform.position.y, TargetPos.z);
            transform.position = Vector3.Lerp(transform.position, newV, FollowSpeed * Time.deltaTime);
        }


        RoarTimer += Time.deltaTime;
        jumpTimer += Time.deltaTime;
        if (RoarTimer > RoarRandomTime && _animator.GetInteger("Attack") == 0)
        {
            RoarTimer = 0;
            _animator.SetInteger("Attack", 1);
            RoarRandomTime = Random.Range(25, 40);
        }
        prehealthTimer += Time.deltaTime;
        if (prehealthTimer > 3)
        {
            prehealth = Health;
            prehealthTimer = 0;
        }
        if (prehealth - Health > 150)
        {
            prehealth = Health;
            StartDefence();
        }
        attackTimer += Time.deltaTime;
        if (Health < MaxHealth / 2)
        {
           ThrowHeadTimer += Time.deltaTime;
            if (ThrowHeadTimer > 45 && _animator.GetInteger("Attack") == 0&&JumpCount == 0)
            {
                ThrowHeadTimer = 0;
                _animator.SetInteger("Attack", 4);
            }
        }
        
        if (attackTimer > 10 && _animator.GetInteger("Attack") == 0)
        {
            attackTimer = 0;
            if (Health < MaxHealth / 2&&Random.Range(0,100) <70)
            {
                    _animator.SetInteger("Attack", 5);
            }
            else
            if (MyPhysics.BoxRayCheck<Mob>(RayOriginTransform.position, 4, new Vector3(-1, 0, 0), 6, 9) != null)
            {

                    _animator.SetInteger("Attack", 3);
     
            }
        }

        if (jumpTimer > 50 && _animator.GetInteger("Attack") == 0)
        {
            jumpTimer = 0;
            StartJump();
        }
    }
    private void FixedUpdate()
    {
        if (IsJumpDown == true)
            rb.velocity = rb.velocity + new Vector3(0, -GravityAdder * Time.fixedDeltaTime, 0);
    }
    public void WhenJumpDown()
    {
        _audioSource.PlayOneShot(thump);
        ObjectPool.Instance.GetObject(hitParticle).transform.position = transform.position;
        CameraAction.Instance.StartShake();
    }
    public void Roar()
    {
        _audioSource.PlayOneShot(roar);
        CameraAction.Instance.StartShake(2, 0.6f);
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            enemys[i].GetComponent<Enemy>().AddBuff(BuffType.SpeedUp, 10);
        }
    }
    public void StartDefence()
    {
        _animator.SetInteger("Attack", 2);
        GetTreatment(100);
        Defence = 20;
        MonoController.Instance.Invoke(4, () => Defence = 0);
    }
    private void HitTheGround()
    {
        _audioSource.PlayOneShot(thump);
        ObjectPool.Instance.GetObject(hitParticle).transform.position = transform.position + new Vector3(-3, 0, 0);
        CameraAction.Instance.StartShake();
        attackBoxes[0].SetActive(true);
        MonoController.Instance.Invoke(0.1f, () => attackBoxes[0].SetActive(false));
    }

    private void SetIntegerToZero()
    {
        _animator.SetInteger("Attack", 0);
    }
    private void StartJump()
    {
        _animator.SetBool("Jump", true);
        MonoController.Instance.Invoke(2, () => _animator.SetBool("Jump", false));
        MonoController.Instance.Invoke(2, () => IsJumpDown = true);
    }
    private void JumpUp()
    {
        JumpCount++;
        rb.velocity = new Vector3(0, JumpForce, 0);

        if (JumpCount > 2)
        {
            MonoController.Instance.Invoke(1f, () => TargetPos = OriginPos);
            JumpCount = 0;
           
        }
        else
        {
            jumpTimer = 42;
            MonoController.Instance.Invoke(1f, () => TargetPos = GameObject.FindGameObjectWithTag("Player").transform.position);

        }
        MonoController.Instance.Invoke(1f, () => ObjectPool.Instance.GetObject(targetCross).transform.position = TargetPos + new Vector3(0, 0.1f, 0));

    }
    private void ThrowZombies()
    {
        
        ThrowOneHead(new Vector3(-2, 1, -2));
        ThrowOneHead(new Vector3(-2, 1, -4)+ new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1)));
        ThrowOneHead(new Vector3(-2, 1, 0) + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
        ThrowOneHead(new Vector3(-2, 1, -6) + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
        ThrowOneHead(new Vector3(-2, 1, -2) + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
        _audioSource.PlayOneShot(SoundSystem.Instance.GetAudioClips("Wave")[0]);
    }
    private void ThrowOneHead(Vector3 Dir)
    {
        GameObject head = Instantiate(ZombieNHead);
        head.transform.position = handTransform.position;
        head.GetComponent<Rigidbody>().velocity = Dir;
    }
    private void CrazyAttack1()
    {
        HitTheGround();
        ShootDirtBullet(new Vector3(-1, 0, 0), 8, 4);
    }
    private void CrazyAttack2()
    {
        HitTheGround();
        ShootDirtBullet(new Vector3(-1, 0, 0.5f), 8, 4);
        ShootDirtBullet(new Vector3(-1, 0, -0.5f), 8, 4);
    }
    private void CrazyAttack3()
    {
        HitTheGround();
        ShootDirtBullet(new Vector3(-1, 0, 0), 8, 4);
        ShootDirtBullet(new Vector3(-1, 0, 1.1f), 8, 4);
        ShootDirtBullet(new Vector3(-1, 0, -1.1f), 8, 4);
    }
    private void CrazyAttack4()
    {
        HitTheGround();
        ShootDirtBullet(new Vector3(-1, 0, -0.5f), 8, 4);
        ShootDirtBullet(new Vector3(-1, 0, 0.5f), 8, 4);
        ShootDirtBullet(new Vector3(-1, 0, 2f), 8, 4);
        ShootDirtBullet(new Vector3(-1, 0, -2f), 8, 4);
    }
    private void ShootDirtBullet(Vector3 dir,float speed,float ac)
    {
        GameObject b =  ObjectPool.Instance.GetObject(DirtBullet);
        b.GetComponent<SandDirtAttack_Boss>().Init(dir, speed, ac);
        b.transform.position = transform.position;
    }
    private void HitTheGroundWhenJumpDown()
    {
        IsJumpDown = false;
        _audioSource.PlayOneShot(thump);
        ObjectPool.Instance.GetObject(hitParticle).transform.position = transform.position;
        CameraAction.Instance.StartShake();
        attackBoxes[1].SetActive(true);
        MonoController.Instance.Invoke(0.1f, () => attackBoxes[1].SetActive(false));
    }
    protected override void OnTriggerEnter(Collider other)//关掉失败检测
    {
        //base.OnTriggerEnter(other);
    }
}
