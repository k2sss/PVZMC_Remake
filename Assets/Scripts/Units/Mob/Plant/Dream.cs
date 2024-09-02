using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream : Plants,IAttacker
{
    public Transform RayTransform;
    private Enemy targetEnemy;
    private Enemy farTargetEnemy;
    public float CloseAttackLength;
    public float FarAttackLength;
    private int WeaponMode = 0;
    public GameObject[] WeaponInHands;
    public GameObject[] WeaponOnBack;//0 Axe 1 Bow 2 Sword
    public GameObject sweep;
    public Transform BowRay;
    public GameObject sheld;
    public float MaxSheldHealth;
    public float SheldHealth;
    public AudioClip breakSound;
    public AudioClip transferSound;
    protected override void Start()
    {
        base.Start();
        if (MaybeUseBool == false)
        {
            SheldHealth = MaxSheldHealth;
            MaybeUseBool = true;
        }
        else
        {
            SheldHealth = MaybeUseTimer;
            if (SheldHealth < 0)
            {
                sheld.SetActive(false);
                CleanAnitiBuff();
                SetAttackSpeed(0.4f);
            }
        }
        
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        if (SheldHealth > 0)
        {
            _animator.SetBool("IsDefence", true);
            MonoController.Instance.Invoke(0.2f,() => _animator.SetBool("IsDefence", false));
            SheldHealth -= Damage;
            MaybeUseTimer = SheldHealth;
            if (SheldHealth <= 0)
            {
                sheld.SetActive(false);
                CleanAnitiBuff();
                PlayOneShot(breakSound);
                SetAttackSpeed(0.4f);
            }
            return;
        }
        base.Hurt(Damage, penetration, Dtype);
    }


    protected override void Update()
    {
        base.Update();
        HurtRig();
        RayCheck();
    }
    private void RayCheck()
    {
        int closeTargetNum = MyPhysics.BoxRayCheckNum<Enemy>(RayTransform.position, 0.5f, new Vector3(1, 0, 0), CloseAttackLength, 8);
        targetEnemy = MyPhysics.BoxRayCheck<Enemy>(RayTransform.position, 0.5f, new Vector3(1, 0, 0), CloseAttackLength , 8);
        WeaponMode = 0;
        if (targetEnemy != null)
        {
            
            if (closeTargetNum <= 1)
            {
                //  Debug.Log("0");
                WeaponMode = 0;
             _animator.SetInteger("IsAttack", 1);
             IsAttacking = true;
            }
            else
            {
                // Debug.Log("1");
                WeaponMode = 2;
                _animator.SetInteger("IsAttack", 3);
              IsAttacking = true;
            }
        }
        else//Èç¹ûÎªnull
        {
            farTargetEnemy = MyPhysics.BoxRayCheck<Enemy>(RayTransform.position, 0.5f, new Vector3(1, 0, 0), FarAttackLength, 8);
            if (farTargetEnemy == null)
            {
                // Debug.Log("2");
                WeaponMode = 0;
                _animator.SetInteger("IsAttack", 0);
                IsAttacking = false;
            }
            else
            {
                // Debug.Log("3");
                WeaponMode = 1;
                _animator.SetInteger("IsAttack", 2);
                IsAttacking = true;
            }
        }
    }
    public void AxeAttack()
    {
        if (targetEnemy != null)
        {
            PlayRandomSounds(SoundSystem.Instance.GetAudioClips("Wave"), 0.7f);
            targetEnemy.Hurt(FinalDamage * 8);
            targetEnemy.Vertigo(0.2f);
            targetEnemy.AddForce(new Vector3(3, 0, 0));
        }
    }
    public void SwordAttack()
    {
        PlayRandomSounds(SoundSystem.Instance.GetAudioClips("Wave"), 0.7f);
        GameObject s = Instantiate(sweep);
        s.transform.position = transform.position + new Vector3(1.4f, 0.5f, 0);
        SwordObj so = s.GetComponent<SwordObj>();
        so.Init(FinalDamage * 5,BuffType.nobuff,0,this);
    }
    public void ShootArrow()
    {
        PlayOneShot(SoundSystem.Instance.GetAudioClips("Bow")[0]);
        GameObject arrowObj = ObjectPool.Instance.GetObject(ResourceSystem.Instance.GetBullet(BulletType.Normal_Arrow));
        arrowObj.transform.position = BowRay.position;
        Bullet bullet = arrowObj.GetComponent<Bullet>();
        
        bullet.init(new Vector3(1, 0, 0), FinalDamage * 5, 15);
        bullet.transform.forward = new Vector3(1, 0, 0);
    }
    public void TransFerToWeponMode()
    {
        PlayOneShot(transferSound);
        SetAWeaponMainWeapon(WeaponMode);
    }
    private void SetAWeaponMainWeapon(int weaponID)
    {
        for (int i = 0; i < WeaponInHands.Length; i++)
        {
            if (i == weaponID)
            {
                WeaponInHands[i].SetActive(true);
            }
           else
            {
                WeaponInHands[i].SetActive(false);
            }

        }
        for (int i = 0; i < WeaponOnBack.Length; i++)
        {
            if (i == weaponID)
            {
                WeaponOnBack[i].SetActive(false);
            }
            else
            {
                WeaponOnBack[i].SetActive(true);
            }

        }



    }
    private void CleanAnitiBuff()
    {
        antiBufftypes = new BuffType[0];
    }

    public void AttackOther(float Damage)
    {
    }

    public float AttackMultiplier()
    {
        return 1;
    }

    public string GetAttackerName()
    {
        return "Dream";
    }
}
