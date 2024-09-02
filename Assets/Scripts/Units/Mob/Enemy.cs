using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
//Animator bool :IsWalk,IsDead,IsAttack
public class Enemy : Mob
{
    [HideInInspector] public bool MaybeUsebool1;
    public EnemyType type;
    [HideInInspector]public Rigidbody rb;
    [HideInInspector] public float Yspeed;
    [Header("SetEnemyInfo")]
    public float ArmorHealth;
    public int Weight = 1;
    public bool CantMove { get; set; }
    protected Mob target;
    private List<EnemyDropItem> dropItems = new List<EnemyDropItem>();
    private float HurtSoundTimer;
    public System.Action onMobDisapear;

    public float JumpForce = 2;
    [HideInInspector] public float jumpTimer;
    public bool IsBoss;
    protected float randomZaxisOffset;
    protected bool isPurified;
    public float aimHeight { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = 8;
        gameObject.tag = "Enemy";
        rb = GetComponent<Rigidbody>();
        randomZaxisOffset = Random.Range(-0.2f, 0.2f);
    }
    protected override void Start()
    {
        base.Start();
        dropItems = ResourceSystem.Instance.GetEnemy(type).dropItems;
        Invoke("Buff", 0.1f);
        aimHeight = ResourceSystem.Instance.GetEnemy(type).aimHeight;
    }

    private void Buff()
    {
        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.EnemyChaoes == true)
        {
            AddBuff(BuffType.Chaoes, 30);
        }
    }

    public void GenerateHpBar()
    {
       
        if(HpBarMgr.Instance ==null)
        {
            GameObject mgr = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("Mgr/HpBarManager"));
            HpBarMgr hpmgr = mgr.GetComponent<HpBarMgr>();
            hpmgr.Init();
           
        }
        HpBarMgr.Instance.Create(this);
    }
    public void ShowDamage(float Damage)
    {
        if (IsVulnerable)
            Damage = Damage * HurtDamageMultiplyerWhenIsVulnerable;
        if (DamageNumMgr.Instance == null)
        {
            GameObject g = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("Mgr/HpBarManager"));
            DamageNumMgr mgr = g.GetComponent<DamageNumMgr>();
            mgr.Init();
        }
        DamageNumMgr.Instance.Create(transform.position, Damage);
    }
    protected override void Update()
    {
        base.Update();
        HurtSoundTimer += Time.deltaTime;
        if(rb != null)
        Yspeed = rb.velocity.y;

        if (isPurified)
        {
            ChangeMaterialColor(new Vector4(0,0.6f,1.6f,1));
        }
    }


    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        if (Health > 0 && HurtSoundTimer >= 0.1f)
        {
            PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).HurtSounds);
            HurtSoundTimer = 0;
        }
        base.Hurt(Damage);
    }
    public override void Death()
    {
        base.Death();
        PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).DeathSounds);
        _animator.SetBool("IsDead", true);
        gameObject.tag = "Untagged";
        EventMgr.Instance.EventTrigger("EnemyCountChange");
    }
    public virtual void DeadSoon(GameObject departBody)
    {
        SoundSystem.Instance.PlayRandom2Dsound(ResourceSystem.Instance.GetEnemy(type).DeathSounds);
        GameObject b = Instantiate(departBody);
        GameObject bl = Instantiate(FileLoadSystem.ResourcesLoad<GameObject>("Particles/Blood"));
        bl.transform.position = transform.position + new Vector3(0,2,0);
        MonoController.Instance.Invoke(2, () => Destroy(bl));
        b.transform.position = transform.position;
        OnDisAppear();
    }
    public virtual void AddForce(Vector3 dir)
    {
        if (Weight < 6)
        {
            rb.velocity = dir + rb.velocity * (1 / (float)Weight);
        }

    }
    public virtual void Vertigo(float time)
    {
        IsVertigo = true;
        MonoController.Instance.Invoke(time, () => IsVertigo = false);
    }
    public virtual void OnDisAppear()//消失时执行
    {

        DropItem();
        onMobDisapear?.Invoke();
        Destroy(gameObject);
    }


    public virtual void Stop()//开停
    {
        _animator.SetBool("IsWalk", false);
        CantMove = true;
    }
    public virtual void Walk()//开走
    {
      
        _animator.SetBool("IsWalk", true);
        CantMove = false;
    }
    public virtual void StartAttack()
    {
        if (IsAttacking == false)
        {
            _animator.SetBool("IsAttack", true);
            CantMove = true;
            IsAttacking = true;
        }
    }
    public virtual void EndAttack()
    {
        if (IsAttacking == true)
        {
            _animator.SetBool("IsAttack", false);
            CantMove = false;
            IsAttacking = false;
        }

    }
    public void DropItem()
    {
        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.MonsterNotFallItem == true)
        {

        }
        else
        for (int i = 0; i < dropItems.Count; i++)
        {
            if (dropItems[i].type != ItemType.Nothing)
            {
                for (int j = 0; j < dropItems[i].maxCount; j++)
                {
                    if (Random.Range(0, 1f) <= dropItems[i].dropProbability||j< dropItems[i].minCount)
                    {
                        GameObject itemObj = Instantiate(ResourceSystem.Instance.GetItem(dropItems[i].type).prefab);
                        Item item = itemObj.GetComponent<Item>();
                        if (item.info.Durability > 1)
                        {
                            item.info.Durability = ResourceSystem.Instance.GetItem(dropItems[i].type).MaxDurability / Random.Range(2, 4);
                        }
                        itemObj.transform.position = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1f), Random.Range(-0.3f, 0.3f));

                    }
                    
                }
                
            }
        }
        GameObject a = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.smoke_burst));
        a.transform.position = transform.position;
    }

    protected void MoveToTheNearestRow()
    {
       
          float z = FindTheNeareastRowPosz();
            if ((transform.position.z - z) > 0.1f + randomZaxisOffset)//在行的上方
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -FinalSpeed);
            }
            else if ((transform.position.z - z) < -0.1f- randomZaxisOffset)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, FinalSpeed);
            }
            rb.velocity = rb.velocity + WaterForce;
    }

    public float FindTheNeareastRowPosz()
    {
        if (EnemyManager.Instance != null)
        {
            float[] rowsZ = new float[EnemyManager.Instance.gameObject.transform.childCount];
            for (int i = 0; i < EnemyManager.Instance.gameObject.transform.childCount; i++)
            {
                rowsZ[i] = EnemyManager.Instance.gameObject.transform.GetChild(i).transform.position.z;
            }
            float distance = Mathf.Infinity;
            float minRowZ = 0;
            for (int i = 0; i < rowsZ.Length; i++)
            {
                if (Mathf.Abs(rowsZ[i] - transform.position.z) < distance)
                {
                    distance = Mathf.Abs(rowsZ[i] - transform.position.z);
                    minRowZ = rowsZ[i];
                }
            }
            return minRowZ;
        }
        return 0;
    }
    public float FindTheRandomRowPosZ()
    {
        if (EnemyManager.Instance != null)
        {
            float[] rowsZ = new float[EnemyManager.Instance.gameObject.transform.childCount];
           
            for (int i = 0; i < EnemyManager.Instance.gameObject.transform.childCount; i++)
            {
                rowsZ[i] = EnemyManager.Instance.gameObject.transform.GetChild(i).transform.position.z;
            }
            if (rowsZ.Length == 1)
            {
                return rowsZ[0];
            }



            float distance = Mathf.Infinity;
            float minRowZ = 0;
            for (int i = 0; i < rowsZ.Length; i++)
            {
                if (Mathf.Abs(rowsZ[i] - transform.position.z) < distance)
                {
                    distance = Mathf.Abs(rowsZ[i] - transform.position.z);
                    minRowZ = rowsZ[i];
                }
            }
            float go = rowsZ[Random.Range(0, rowsZ.Length)];
            while (go == minRowZ)
            {
                go = rowsZ[Random.Range(0, rowsZ.Length)];
            }
            return go;
        }
       
        return 0;
    }
    

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckCollider") && IsDead == false)
        {

            EventMgr.Instance.EventTrigger("GameOver");
        }
    }

}
[System.Serializable]
public class EnemyDropItem
{
    public ItemType type;
    public int maxCount;
    public int minCount;
    public float dropProbability = 1;

}