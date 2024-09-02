using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Audio;
public class Mob : MonoBehaviour//Mob主要负责生物的通用功能
{
    [Header("BasicInformation")]
    public float Health;
    public float MaxHealth;
    public float Defence;
    [Header("AnimationRigging_Constraint")]
    public float HurtRigStrength = 20;
    public float HurtHoldTime = 0.1f;
    public float MaxConstraintWeight = 0.8f;
    public MultiRotationConstraint hurtConstraint;
    protected Animator _animator;
    protected float ConstraintTargetWeight = 0;
    protected float ConstraintWeight;
    [Header("HurtChangeColor")]
    public List<Renderer> renderers = new List<Renderer>();
    public Vector4 HurtColor = new Vector4(3, 0, 0, 1);
    [HideInInspector] public bool IsDead;

    [Header("Sound")]
    protected AudioSource _audioSource;

    protected Collider _collider;
    public MobBuff mobBuffer { get; private set; }
    //事件
    public System.Action onMobDead;
    public System.Action onMobHealthChange;
    public BuffType[] antiBufftypes;
    public bool IsVertigo { get; set; }
    public bool IsPoisoned { get; set; }
    public bool IsDry { get; set; }
    public bool IsChaoes { get; set; }

    public bool IsVulnerable { get; set; }

    public bool IsCrimsonDecay { get; set; }
    public bool IsInWater { get; set; }

    public Vector3 WaterForce;
    public int WaterGridCount { get; set; }

    private float DecayTimer;
    private float PoiTimer;
    private float Colortimer;
    protected bool IsAttacking;

    public float BaseSpeed;
    private float MoveSpeedMultiplier = 1;
    [HideInInspector] public float FinalSpeed;
    public float BaseDamage;
    [HideInInspector] public float FinalDamage;
    private float DamageMultiplier = 1;
    private float AnimatorSpeedMultiplier = 1;
    protected float AttackSpeedMultiplier = 1;
    protected const float HurtDamageMultiplyerWhenIsVulnerable = 1.5f;

    protected virtual void Awake()
    {

        mobBuffer = GetBuffer();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }
    protected virtual void Start()
    {
        _audioSource = SoundSystem.Instance.InitAudioSource(gameObject, SoundSystem.Instance.SoundMixer);
        SetNowDamage(0);
        SetNowSpeed(0);
        
    }

    protected virtual void Update()
    {
        Colortimer += Time.deltaTime;
        if (Colortimer > 0.5f)
        {
            Colortimer = 0;
            ChangeMaterialColor(new Vector4(1, 1, 1, 1));
        }
        if(transform.position.y < -10)
        {
            transform.position += Vector3.up * 20;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (IsDead == false)
        {
            MaterialAction();
            AnimationAction();

        }


    }
    private void MaterialAction()
    {
        if (IsDry == true)
        {
            ChangeMaterialColor(new Vector4(0.8f, 0.2f, 0.1f, 1));
        }
        if (IsCrimsonDecay)
        {
            ChangeMaterialColor(new Vector4(1f, 0.1f, 0.1f, 1));
            DecayTimer += Time.deltaTime;
            if (DecayTimer > 0.5f)
            {
                Hurt(3, 0, DamageType.trueDamage);
                DecayTimer = 0;
            }
        }



        if (IsPoisoned == true)
        {
            ChangeMaterialColor(new Vector4(0.7f, 1, 0.7f, 1));
            PoiTimer += Time.deltaTime;
            if (PoiTimer >= 1)
            {
                PoiTimer = 0;
                if (Health > 1)
                    Hurt(1, 0, DamageType.trueDamage);

            }
        }
        if (IsChaoes == true)
        {
            ChangeMaterialColor(new Vector4(0, 0, 0, 1));
        }
        if (IsVulnerable == true)
        {
            ChangeMaterialColor(new Vector4(5, 5, 5, 1));
        }
    }
    private void AnimationAction()
    {
     if (_animator == null)
                return;
            if (IsAttacking == false)
            {
                if (AnimatorSpeedMultiplier < 0.1f)
                {
                    _animator.speed = 0.1f;
                }
                else
                {
                    _animator.speed = AnimatorSpeedMultiplier;
                }
            }
            else
            {

                if (AttackSpeedMultiplier < 0.1f)
                {
                    _animator.speed = 0.1f;
                }
                else
                {
                    _animator.speed = AttackSpeedMultiplier;
                }

            }
          
    }
    #region hurt
    private bool AntiBuff(BuffType type)
    {
        for (int i = 0; i < antiBufftypes.Length; i++)
        {
            if (antiBufftypes[i] == type)
            {
                return true;
            }
        }
        return false;
    }
    public void AddBuff(BuffType type, float time)
    {
        if (mobBuffer == null)
        {
            mobBuffer = GetBuffer();
        }
        if (mobBuffer != null && IsDead == false && !AntiBuff(type))
        {
            mobBuffer.AddBuff(type, time);
        }
    }
    public void AddBuff(BuffType type)
    {
        if (mobBuffer == null)
        {
            mobBuffer = GetBuffer();
        }
        if (mobBuffer != null && IsDead == false && !AntiBuff(type))
        {
            mobBuffer.AddPersistentBuff(type);
        }
    }
    public void StopBuff(BuffType type)
    {
        if (mobBuffer != null)
        {
            mobBuffer.StopBuff(type);
        }
    }
    public virtual void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        if (Damage <= 0 || penetration < 0)
        {
            return;
        }
        if (IsVulnerable == true)
        {
            Damage *= HurtDamageMultiplyerWhenIsVulnerable;
        }

        float finalDamage = 0;
        if (Dtype == DamageType.normal)
        {
            finalDamage = Damage * (1f - (Defence) * 0.04f) + penetration;
            if (finalDamage > Damage)
            {
                finalDamage = Damage;
            }
        }
        if (Dtype == DamageType.trueDamage)
        {
            finalDamage = Damage;
        }
       
        Health -= finalDamage;
       
        ConstraintTargetWeight = MaxConstraintWeight;
        ChangeMaterialColor(HurtColor);
        Invoke("AfterHurt", HurtHoldTime);
        if (Health < 1 && IsDead == false)
        {
            Death();
        } 
        onMobHealthChange?.Invoke();
    }

    public virtual void Hurt(float Damage, IAttacker attcker,float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        Hurt(Damage,penetration, Dtype);
    }
    protected virtual void AfterHurt()//受伤后恢复原状
    {
        ConstraintTargetWeight = 0;
        if (GetMaterialColor() == HurtColor)
            ChangeMaterialColor(Color.white);
    }
    public virtual void Death()
    {
        IsDead = true;
        onMobDead?.Invoke();
    }

    protected virtual void HurtRig()
    {

            ConstraintWeight = Mathf.Lerp(ConstraintWeight, ConstraintTargetWeight, HurtRigStrength * Time.deltaTime);
            hurtConstraint.weight = ConstraintWeight;

    }
    #endregion
    #region material
    public virtual void ChangeMaterialColor(Vector4 color)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material.color = color;
        }
        for (int i = 0; i < renderers.Count; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                renderers[i].materials[j].color = color;
            }
        }

    }
    public virtual void ChangeMaterial(Material mat)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material = mat;
        }
    }

    public Vector4 GetMaterialColor()
    {
        return renderers[0].material.color;
    }
    #endregion
    #region sound
    public void PlayRandomSounds(AudioClip[] clips, float value = 1)
    {
        _audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)], value);
    }
    public void PlayOneShot(AudioClip audioclip, float value = 1)
    {
        _audioSource.PlayOneShot(audioclip, value);
    }
    public void PlayAudioClip(AudioClip clip)
    {

        _audioSource.PlayOneShot(clip);
    }
    #endregion
    #region Find
    public GameObject FindTheNearestMob(string tag, float Range)
    {
        GameObject[] allmobs = GameObject.FindGameObjectsWithTag(tag);
        GameObject go = null;
        float mindistance = Mathf.Infinity;
        foreach (GameObject a in allmobs)
        {
            //比较大小
            float length = (transform.position - a.transform.position).magnitude;
            if (length < mindistance && length <= Range)
            {
                mindistance = length;
                go = a;
            }
        }
        return go;
    }
    public GameObject[] FindTheNearestMobs(string tag, float Range)
    {
        GameObject[] allmobs = GameObject.FindGameObjectsWithTag(tag);
        List<GameObject> mobs = new List<GameObject>();
        foreach (GameObject a in allmobs)
        {
            //比较大小
            float length = (transform.position - a.transform.position).magnitude;
            if (length <= Range)
            {
                mobs.Add(a);
            }
        }
        return mobs.ToArray();

    }
    public GameObject FindTheNearestMob(string tag, float Xrange, float Zrange)
    {
        GameObject[] allmobs = GameObject.FindGameObjectsWithTag(tag);
        GameObject go = null;
        float mindistance = Mathf.Infinity;
        foreach (GameObject a in allmobs)
        {
            //比较大小
            float x = Mathf.Abs(transform.position.x - a.transform.position.x);
            float z = Mathf.Abs(transform.position.z - a.transform.position.z);
            if (x < mindistance && z <= Zrange && x <= Xrange)
            {
                mindistance = x;
                go = a;
            }
        }
        return go;
    }
    #endregion
    public bool WithInRange(float range, Transform other)
    {
        if ((transform.position - other.position).magnitude <= range)
        {
            return true;
        }
        else
            return false;
    }
    private MobBuff GetBuffer()
    {
        MobBuff mobbuffer = gameObject.GetComponent<MobBuff>();
        if (mobbuffer != null)
        {
            return mobbuffer;
        }
        else
        {
            mobbuffer = gameObject.AddComponent<MobBuff>();
            return mobbuffer;
        }
    }
    #region SetAttribute
    public void SetNowSpeed(float addvalue)
    {
        MoveSpeedMultiplier += addvalue;
        if (MoveSpeedMultiplier < 0.1f)
        {
            FinalSpeed = 0.1f * BaseSpeed;
        }
        else
        {
            FinalSpeed = BaseSpeed * MoveSpeedMultiplier;
        }
    }
    public void SetAnimatorSpeed(float addvalue)
    {
        AnimatorSpeedMultiplier += addvalue;
    }
    public void SetAttackSpeed(float addvalue)
    {
        AttackSpeedMultiplier += addvalue;
    }

    public void SetNowDamage(float addValue)
    {

        DamageMultiplier += addValue;
        if (DamageMultiplier < 0.1f)
        {
            FinalDamage = 0.1f * BaseDamage;
        }
        else
        {
            FinalDamage = BaseDamage * DamageMultiplier;
        }

    }
    #endregion
    public virtual void GetTreatment(float value,bool isShowingEffects = true)
    {
        if (Health < MaxHealth)
        {
            if (Health + value <= MaxHealth)
            {
                Health += value;
                
            }
            else
            {
                Health = MaxHealth;
            }
            onMobHealthChange?.Invoke();
            if (isShowingEffects)
            { GameObject p = ObjectPool.Instance.GetObject(ResourceSystem.Instance.GetParticle(ParticleType.Glint));
            p.transform.position = transform.position;
            ChangeMaterialColor(new Vector4(0.3f, 1.4f, 0.3f, 1));
            MonoController.Instance.Invoke(0.3f, () => ChangeMaterialColor(Color.white));

            }

           

        }

    }

}
