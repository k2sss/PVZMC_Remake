using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MobBuff : MonoBehaviour
{
    public List<Buff_Scriptable> buffs = new List<Buff_Scriptable>();

    protected Mob mob;
    BuffType[] Bufftypes;
    private GameObject blackSmokeParticle;
    private GameObject targetblackSmokeParticle;
    private GameObject decayParticle;
    private GameObject tardecay;
    private GameObject AttackSpeedUpParticle;
    private GameObject tarAttackSpeedUpParticle;
    private GameObject SpeedUpParticle;
    private GameObject solidParitcle;
    private GameObject solidParticlePrefab;

    protected virtual void Awake()
    {
        Bufftypes = (BuffType[])Enum.GetValues(typeof(BuffType));
        for (int i = 0; i < Bufftypes.Length; i++)
        {
            buffs.Add(new Buff_Scriptable(Bufftypes[i]));
        }
    }

    public virtual void Start()
    {
        mob = GetComponent<Mob>();
       
        blackSmokeParticle = FileLoadSystem.ResourcesLoad<GameObject>("Particles/BlackSmoke");
        AttackSpeedUpParticle = FileLoadSystem.ResourcesLoad<GameObject>("Particles/AttackSpeedUpSmoke");
        SpeedUpParticle = FileLoadSystem.ResourcesLoad<GameObject>("Particles/SpeedUpParticle");
        decayParticle = FileLoadSystem.ResourcesLoad<GameObject>("Particles/Decay");
        solidParticlePrefab = Resources.Load<GameObject>("Particles/SolidParticle");
        InitBuffFunc();
    }
    protected virtual void InitBuffFunc()//初始化
    {
       //Dry
        buffs[2].onStartWork += () => mob.SetNowSpeed(-0.4f);
        buffs[2].onStartWork += () => mob.SetAnimatorSpeed(-0.4f);
        buffs[2].onStartWork += () => mob.IsDry = true;
        buffs[2].onStopWork += () => mob.SetNowSpeed(+0.4f);
        buffs[2].onStopWork += () => mob.SetAnimatorSpeed(+0.4f);
        buffs[2].onStopWork += () => mob.IsDry = false;

        //Poison
        buffs[3].onStartWork += () => mob.IsPoisoned = true;
        buffs[3].onStopWork += () => mob.IsPoisoned = false;
        //Chaoes
        buffs[5].onStartWork += () => mob.IsChaoes = true;
        buffs[5].onStartWork += () => mob.SetNowSpeed(0.3f);
        buffs[5].onStartWork += () => mob.SetNowDamage(0.4f);
        buffs[5].onStartWork += () =>InitParticle(ref targetblackSmokeParticle,blackSmokeParticle);
        if(mob.CompareTag("Player"))
        buffs[5].onStartWork += ()=>SoundSystem.Instance.Play2Dsound("Stare");
        buffs[5].onStopWork += () => mob.IsChaoes = false;
        buffs[5].onStopWork += () => mob.SetNowSpeed(-0.3f);
        buffs[5].onStopWork += () => mob.SetNowDamage(-0.4f);
        buffs[5].onStopWork += () => Destroy(targetblackSmokeParticle);
        //AttackSpeedU
        buffs[6].onStartWork += () => mob.SetAttackSpeed(1);
        buffs[6].onStopWork += () => mob.SetAttackSpeed(-1);
        buffs[6].onStartWork += () => InitParticle(ref tarAttackSpeedUpParticle, AttackSpeedUpParticle);
        buffs[6].onStopWork += () => Destroy(tarAttackSpeedUpParticle);
        //加速
        buffs[7].onStartWork += () => mob.SetNowSpeed(0.3f);
        buffs[7].onStartWork += () => mob.SetAnimatorSpeed(0.3f);
        buffs[7].onStopWork += ()=> mob.SetNowSpeed(-0.3f);
        buffs[7].onStopWork += () => mob.SetAnimatorSpeed(-0.3f);
        buffs[7].onStartWork += () => ObjectPool.Instance.GetObject(SpeedUpParticle).transform.position = transform.position + new Vector3(0, 0.5f, 0);
        //泰坦套装
        buffs[8].onStartWork += () => mob.SetNowSpeed(0.1f);
        buffs[8].onStopWork += () => mob.SetNowSpeed(-0.1f);
        buffs[8].onStartWork += () => mob.SetNowDamage(0.1f);
        buffs[8].onStopWork += () => mob.SetNowDamage(-0.1f);
        //急速
        buffs[9].onStartWork += () => mob.SetNowSpeed(1.5f);
        buffs[9].onStartWork += () => mob.SetAnimatorSpeed(1.5f);
        buffs[9].onStopWork += () => mob.SetNowSpeed(-1.5f);
        buffs[9].onStopWork += () => mob.SetAnimatorSpeed(-1.5f);
        //易伤
        buffs[10].onStartWork += () =>
        {
            mob.IsVulnerable = true;
        };
        buffs[10].onStopWork += () =>
        {
            mob.IsVulnerable = false;
        };
        //猩红腐败
        buffs[11].onStartWork += () =>
        {
            mob.IsCrimsonDecay = true;
            mob.SetAnimatorSpeed(1.2f);
            mob.SetAttackSpeed(1.2f);
            InitParticle(ref tardecay, decayParticle);

        };
        buffs[11].onStopWork += () =>
        {
            mob.IsCrimsonDecay = false;
            mob.SetAnimatorSpeed(-1.2f);
            mob.SetAttackSpeed(-1.2f);
            Destroy(tardecay);

        };
        //净化
        buffs[12].onStartWork += () =>
        {
            Ipurefiable ipf = mob as Ipurefiable;
            ipf.OnPurify();
        };
        buffs[12].onStopWork += () =>
        {
            Ipurefiable ipf = mob as Ipurefiable;
            ipf.OnEndPurify();
        };
        buffs[18].onStartWork += () =>
        {
            mob.SetNowSpeed(0.3f);
            mob.SetAnimatorSpeed(0.3f);
        };
        buffs[18].onStopWork += () =>
        {
            mob.SetNowSpeed(-0.3f);
            mob.SetAnimatorSpeed(-0.3f);
        };
        buffs[19].onStartWork += () =>
        {
            mob.Defence += 10;
            solidParitcle = Instantiate(solidParticlePrefab,transform);
            solidParitcle.transform.position = transform.position;
        };
        buffs[19].onStopWork += () =>
        {
            mob.Defence -= 10;
            Destroy(solidParitcle);
        };

    }
    private void InitParticle(ref GameObject tar,GameObject par)
    {
        tar = Instantiate(par, transform);
        tar.transform.position = transform.position;
    }
    private void Update()
    {
        foreach (Buff_Scriptable buff in buffs)
        {
            buff.UpdateTimer();
        }
    }
    public void AddBuff(BuffType type, float time)//添加一个buff
    {
        //foreach (Buff_Scriptable buff in buffs)
        //{
        //    if (type == buff.type)
        //    {
        //        buff.GainThisBuff(time);
        //        break;
        //    }
        //}
        buffs[(int)type].GainThisBuff(time);
    }
    public void AddPersistentBuff(BuffType type)
    {

        //foreach (Buff_Scriptable buff in buffs)
        //{
        //    if (type == buff.type)
        //    {

        //        buff.GainThisBuff();
        //        break;
        //    }
        //}
        buffs[(int)type].GainThisBuff();
    }
    public void StopBuff(BuffType type)
    {
        buffs[(int)type].StopThisBuff();
    }
    public void StopAllBuff()
    {
        foreach (Buff_Scriptable buff in buffs)
        {
            buff.StopThisBuff();
        }

    }

    public bool IsContainBuff(BuffType type)
    {
        return buffs[(int)type].IsWorking() ? true:false;
    }
}

