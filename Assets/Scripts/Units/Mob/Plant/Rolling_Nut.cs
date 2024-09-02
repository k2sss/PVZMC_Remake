using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rolling_Nut : Plants
{
    private Rigidbody rb;
    public bool isChangeDirection = true;
    public bool isExplode;
    public bool isBig;
    public GameObject explodeGameObject;
    public AudioClip[] soundclips;

    private AudioSource Soundsource;
    private float AttackCD;
    private float Xspeed;

    private float Zspeed;
    private float ZspeedTarget;
    private float MaxZ;
    private float MinZ;
    private float MaxX;
    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = 0;
        gameObject.tag = "Untagged";
    }
    protected override void Start()
    {
        base.Start();
        AttackCD = 0.2f;
        Soundsource = SoundSystem.Instance.InitAudioSource(gameObject, SoundSystem.Instance.SoundMixer);
        PlayOneShot(soundclips[0]);
        rb = GetComponent<Rigidbody>();

        Xspeed = FinalSpeed;
        ZspeedTarget = FinalSpeed / 1.2f;

        GridManager gridm = GridManager.Instance;
        MaxZ = gridm.transform.position.z + gridm.width * 2;
        MinZ = gridm.transform.position.z;
        MaxX = gridm.GetMaxX();

        if (targetGrid != null)
            targetGrid.RemovePlantBind();//灰烬类植物需要解开与GRID的绑定

    }
    protected override void Update()
    {
        base.Update();
        AttackCD += Time.deltaTime;

        //检测是否越界
        //Debug.Log(MaxZ);
        if (transform.position.z > MaxZ && Zspeed > 0)
        {
            ChangeDirection();
        }
        if (transform.position.z < MinZ && Zspeed < 0)
        {
            ChangeDirection();
        }
        if (transform.position.x > MaxX + 3)
        {
            Destroy(gameObject);
        }

    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(Xspeed, 0, Zspeed);
    }
    private void ChangeDirection()
    {
        if (isChangeDirection == true)
        {
            Xspeed = FinalSpeed / 1.6f;
            if (Zspeed == 0f)
            {

                if (Random.Range(0, 2) == 0)
                {
                    Zspeed = ZspeedTarget;
                }
                else
                {
                    Zspeed = -ZspeedTarget;
                }
            }
            else if (Zspeed > 0f)
            {
                Zspeed = -ZspeedTarget;
            }
            else
            {
                Zspeed = ZspeedTarget;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isBig == true)
        {
            if (other.CompareTag("Enemy"))
            {
                PlayOneShot(soundclips[1]);
                other.GetComponent<Enemy>().Hurt(FinalDamage);
            }

        }
        else
        {
            if (other.CompareTag("Enemy") && AttackCD > 0.1f)
            {
                if (isExplode == true)
                {
                    //爆炸
                    CameraAction.Instance.StartShake();
                    GameObject e = ObjectPool.Instance.GetObject(explodeGameObject);
                    e.transform.position = transform.position;
                    e.GetComponent<ExplodeBox>().InitDamage(0, 250, 0, 250);

                    Destroy(gameObject);

                }
                else
                {
                    PlayOneShot(soundclips[1]);
                    other.GetComponent<Enemy>().Hurt(FinalDamage);
                    ChangeDirection();
                    AttackCD = 0;
                }
            }
        }


    }
}
