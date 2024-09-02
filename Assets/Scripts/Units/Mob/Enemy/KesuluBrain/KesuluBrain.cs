using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MyClass.FSM.Kesulu;
using MyClass.Extensions.TransformExtension;

public class KesuluBrain : Enemy
{
    [SerializeField] private Vector3 EnterPosition;
    [SerializeField] private Vector3 OriginPosition;
    [SerializeField] private Transform ShootPosition;
    [SerializeField] private GameObject BloodBombObj;
    [SerializeField] private GameObject DownAttackParticle;
    [SerializeField] private RuntimeAnimatorController animator_Phase2;
    [SerializeField] private GameObject BreathBullet;
    [SerializeField] private GameObject bloodBurstParticle;

    private Brain_FSM<Enemy> fsm;

    public Animator thisAnimator { get; private set; }
    [SerializeField] private AudioClip BossRoar;
    [SerializeField] private AudioClip ThorwBombSound;
    private float targetFindTimer;
    private float targetFindInterval = 15;
    private float playerDamageCount;
    private Mob playertarget;
    private float SelfRange = 3;
    private int guardCount = 0;
    private bool isSheilded;
    public int HeartBreakCount { get; private set; }
    protected override void Start()
    {
        base.Start();
        transform.position = EnterPosition;
        fsm = new Brain_FSM<Enemy>(this);

        playertarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Mob>();
        EventMgr.Instance.AddEventListener("HEART", () =>
        {
            string textContent = "";
            if (HeartBreakCount == 0)
                textContent = "不寒而栗，毛骨悚然...";
            else if (HeartBreakCount == 1)
                textContent = "四面回荡着尖叫声";
            InformationMessage.Instance.WriteMessage(textContent, new Color(0.1f, 0.6f, 0.1f, 0));
            HeartBreakCount++;
        });

        GetCompoent();

    }
    
    private void GetCompoent()
    {
        thisAnimator = gameObject.GetComponent<Animator>();
    }
    private void NextStep()
    {
        //如果为一阶段
        if (fsm.GetCurrentState() is Phase1<Enemy>)
        {
            Phase1<Enemy> PHASE1state = (Phase1<Enemy>)fsm.GetState((int)Brain_FSM<Enemy>.StateType.PHASE1);
            PHASE1state.NextMotion();
        }
        //如果为二阶段
        if (fsm.GetCurrentState() is Phase2<Enemy>)
        {
           
            Phase2<Enemy> PHASE2state = (Phase2<Enemy>)fsm.GetState((int)Brain_FSM<Enemy>.StateType.PHASE2);
            PHASE2state.NextMotion();
        }

    }
    public override void ChangeMaterialColor(Vector4 color)
    {
        renderers[0].material.SetColor("_Color", color);
    }
    protected override void Update()
    {
        base.Update();
        fsm.Update();
        TargetFindTimerLogic();

    }
   
    public void ChangeAnimatorControllerToPhase2()
    {
        thisAnimator.runtimeAnimatorController = animator_Phase2;
    }

    public Vector3 GetOriginPos()
    {
        return OriginPosition;
    }
    //出场
    public void EnterScene()
    {

        SoundSystem.Instance.Play2Dsound(BossRoar);
        MusicMgr.Instance.PlayMusic("Boss1");
        MusicMgr.Instance.IsLock = true;
        InformationMessage.Instance.WriteMessage("克苏鲁之脑 已苏醒", new Color(0.6f, 0.1f, 0.6f));
        transform.position = EnterPosition;
        transform.DOMove(OriginPosition, 3);

    }
    //根据当前状态选择目标
    #region targetRegion
    public Mob GetTarget()
    {
        return target;
    }
    private void FindTarget()
    {
        if (playertarget == null)
            return;

        if (Random.Range(0, 400) + playerDamageCount > 300)
        {
            playerDamageCount = 0;
            target = playertarget;

            return;
        }
        //随机找植物
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plants");
        if (plants.Length == 0)
        {
            target = playertarget;

            return;
        }
        target = plants[Random.Range(0, plants.Length)].GetComponent<Mob>();

    }
    public bool IsTargetFarAway()
    {
        if (target == null) return false;
        if ((transform.position - target.transform.position).magnitude >= SelfRange)
            return true;

        return false;
    }
    public bool IsTargetNearBy()
    {
        if (target == null) return false;
        if ((transform.position - target.transform.position).magnitude < SelfRange)
            return true;
        return false;
    }
    public bool IsTargetForward()
    {
        if (target == null) return false;
        Vector3 direction = transform.position - target.transform.position;
        if (Mathf.Abs(direction.z) <= 1f)
        {
            return true;
        }

        return false;
    } 
    private void TargetFindTimerLogic()
    {
        if (target == null || target.IsDead)
            FindTarget();

        targetFindTimer += Time.deltaTime;
        if (targetFindTimer > targetFindInterval)
        {
            targetFindTimer = 0;
            FindTarget();
        }
    }
    #endregion

    public override void Death()
    {
        base.Death();
        SoundSystem.Instance.Play2Dsound(BossRoar);
        GameObject p = Instantiate(bloodBurstParticle);
        p.transform.position = transform.position + Vector3.up;
        OnDisAppear();
        
    }

    public override void Hurt(float Damage, IAttacker attacker, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
       
        base.Hurt(Damage, penetration, Dtype);
       
        ShowDamage(Damage);
        
        if (attacker.GetAttackerName().Equals("Player"))
        {

            playerDamageCount += Damage;
            playerDamageCount = Mathf.Clamp(playerDamageCount, 0, 200);
        }
    }
    #region AttackRogic
    private void SendEye()
    {
        if (target == null) return;
        if (ShootPosition == null) return;
        EyeCreeper_BulletType eyebullet = EnemyManager.Instance.CreateEnemy(EnemyType.eyeCreeper2, ShootPosition.position) as EyeCreeper_BulletType;
        eyebullet.SetTarget(target);
        SoundSystem.Instance.Play2Dsound(ThorwBombSound);
    }
    private void CallEnemy()
    {
        for (int i = 0; i < 10; i++)
        {
            EnemyManager.Instance.CreateEnemyInRandomRow(EnemyType.eyeCreeper, -4);
        }
        Enemy gurad = EnemyManager.Instance.CreateEnemyInRandomRow(EnemyType.eyeCreeper, -4);
        (gurad as EyeCreeper_NormalType).ChangeType();
        guardCount++;
        EnterSheild();
        gurad.onMobDisapear +=
        () => 
        {
            guardCount--;
            if(guardCount == 0)
            QuitSheild();
        };
        SoundSystem.Instance.Play2Dsound(BossRoar, 0.4f);
        CameraAction.Instance.StartShake();
    }
    private void ThorwBomb()
    {
        if (BloodBombObj == null) return;
        if (target == null) return;

        SoundSystem.Instance.Play2Dsound(ThorwBombSound);

        GameObject bomb = ObjectPool.Instance.GetObject(BloodBombObj);
        bomb.transform.position = ShootPosition.position;
        bomb.GetComponent<BloodBomb>().SetPos(ShootPosition.position, target.transform.position);
    }
    private void DownAttack()
    {
        List<Mob> mobs = transform.GetNearbyPlants(SelfRange);
       
        for (int i = 0; i < mobs.Count; i++)
        {

            if (mobs[i] is Plants)
            mobs[i].Hurt(200);
            else
            mobs[i].Hurt(15);
            mobs[i].AddBuff(BuffType.reverse,5);
        }
        //粒子效果
        GameObject particle = ObjectPool.Instance.GetObject(DownAttackParticle);
        particle.transform.position = transform.position;
        SoundSystem.Instance.Play2Dsound("Shock");
        CameraAction.Instance.StartShake();
    }
    private void EnterSheild()
    {
        if (isSheilded) return;

        renderers[0].material.SetFloat("_addFloat", 3);
        Defence = 20;
        isSheilded = true;
    }
    private void QuitSheild()
    {
        if (!isSheilded) return;

        renderers[0].material.SetFloat("_addFloat", 0);
        Defence = 0;
        isSheilded = false;
    }
    
    private void CallBuff()
    {
        GameObject[] enemyObjects= EnemyManager.Instance.enemys;
        foreach (var item in enemyObjects)
        {
            var e = item.GetComponent<Enemy>();
            if (e is KesuluBrain)
                continue;
            e.AddBuff(BuffType.Chaoes,10);
            e.AddBuff(BuffType.SpeedUp, 10);
        }


        SoundSystem.Instance.Play2Dsound(BossRoar, 0.4f);
        CameraAction.Instance.StartShake();
    }
    private void CrimsonBreath()
    {
        GameObject breathObject = ObjectPool.Instance.GetObject(BreathBullet);
        breathObject.transform.position = transform.position;
    }
    private void ShootLaser()
    {

    }
    private void ShakeCamera(float time)
    {
        CameraAction.Instance.StartShake(time);
    }

    #endregion
}
