using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerHealth : Mob, IWater,IAttacker
{
    public static PlayerHealth instance;
    public float MaxGoldenHealth;
    public float GoldenHealth;
    public AudioClip[] hurtSounds;
    public Action onPlayerHealthChange;
    public float isInvincibletimer { get; private set; }
    private ArmorSlot[] armorSlots;

    public float natureAddHealthValue { get; set; }
    

    protected override void Awake()
    {
        base.Awake();
        instance = this;
        FinalSpeed = BaseSpeed;
    }
    protected override void Start()
    {
        base.Start();
       

        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.PlayerDisappear == true)
        {
            gameObject.SetActive(false);
        }
        if (InventoryManager.Instance != null)
        {
            Transform parent = InventoryManager.Instance.transform.Find("Inventory/Inventory_Main/ArmorSlots");
            armorSlots = new ArmorSlot[parent.childCount];
            for (int i = 0; i < parent.childCount; i++)
            {
                armorSlots[i] = parent.GetChild(i).GetComponent<ArmorSlot>();
            }

        }
        InvokeRepeating("NatureRecover", 0, 4);
    }
    private void NatureRecover()
    {
        AddHealth(natureAddHealthValue);
    }

    public void ChangeMaxHealth(float maxHealth)
    {
        this.MaxHealth =maxHealth;
        EventMgr.Instance.EventTrigger("max_hp_change");
    }
    public override void GetTreatment(float value,bool IsShowingEffects = true)
    {
        base.GetTreatment(value,IsShowingEffects);
        onPlayerHealthChange?.Invoke();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        isInvincibletimer -= Time.deltaTime;
        HurtRig();
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        if (isInvincibletimer < 0)
        {
            isInvincibletimer = 0.2f;
            base.Hurt(Damage,penetration,Dtype);
            OnPlayerHurt();
            PlayRandomSounds(hurtSounds);
            CameraAction.Instance.StartShake(0.3f, 0.4f, 10);
            if(Dtype != DamageType.trueDamage)
            for (int i = 0; i < armorSlots.Length; i++)
            {
                if (armorSlots[i].info.type != ItemType.Nothing)
                {
                    armorSlots[i].info.Durability -= 1;
                    armorSlots[i].ItemUpdate();
                    EventMgr.Instance.EventTrigger("UpdateArmor");
                }
            }


        }

    }
    public override void Death()
    {
        base.Death();
        if (!MySystem.IsInLevel())
        {
            SceneMgr.Instance.LoadAsync(SceneMgr.Instance.GetSceneName());
        }
        EventMgr.Instance.EventTrigger("GameOver");
        _animator.SetBool("Dead", true);

    }
    public void Disappear()
    {
        GameObject p = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.smoke_burst));
        p.transform.position = transform.position;
        gameObject.SetActive(false);
    }
    private void OnPlayerHurt()
    {
        onPlayerHealthChange?.Invoke();
    }
    public bool IsInRange(Vector3 point, float Range)
    {
        if ((point - transform.position).magnitude <= Range)
        {
            return true;
        }
        return false;

    }
    public void AddHealth(float value)
    {
        if (Health < MaxHealth)
        {
            Health += value;
            onPlayerHealthChange?.Invoke();
        }
    }
    public void ReFresh()
    {
        onPlayerHealthChange?.Invoke();
    }

    public void IEnterWater()
    {
        PlayerMoveController.Instance.IsSwimming = true;
    }

    public void IExitWater()
    {
        PlayerMoveController.Instance.IsSwimming = false;
    }

    public void AttackOther(float Damage)
    {
        //如果有吸血能力
        if(mobBuffer.IsContainBuff(BuffType.VampireAbility))
        {
            GetTreatment(Damage * 0.1f,false);
        }
    }

    public float AttackMultiplier()
    {
        if (mobBuffer.IsContainBuff(BuffType.ProbabilisticDamage))
        {
           if(UnityEngine.Random.Range(0,100) <= 25)
            {
                return 2;
            }
        }
        return 1;
    }

    public string GetAttackerName()
    {
        return "Player";
    }
}
