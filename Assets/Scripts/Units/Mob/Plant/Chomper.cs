using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : Plants
{
    public Transform AttackRayTransform;
    public float RayLength;
    public AudioClip[] clips;
    public float ChewTime = 20;
    private Enemy TargetEnemy;
    public GameObject BloodParticle;
    public GameObject Particle2;
    private float TreatTimer;
    //maybeUsEtIMER == chewTimer,maybeusebool == ischew
    protected override void Start()
    {
        base.Start();
        BloodParticle.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        RayCheck();
        if (MaybeUseBool == true)
        {   
            StartChew();
            TreatTimer += Time.deltaTime;
            if (TreatTimer > 4)
            {
                TreatTimer = 0;
                GetTreatment(2);
            }
            MaybeUseTimer += Time.deltaTime;
            if (MaybeUseTimer > ChewTime)
            {
                EndChew();
                TreatTimer = 0;
            }
           
        }
    }
    private void RayCheck()
    {
        TargetEnemy = MyPhysics.BoxRayCheck<Enemy>(AttackRayTransform.position, 0.5f, new Vector3(1, 0, 0), RayLength, 8);
        if (TargetEnemy != null)
        {
            _animator.SetBool("IsAttack", true);
            IsAttacking = true;

        }
        else
        {
            _animator.SetBool("IsAttack", false);
            IsAttacking = false;
        }
    }
    public void Eat()
    {
        if (TargetEnemy != null)
        {
            PlayOneShot(clips[0]);
            if (TargetEnemy.MaxHealth > 250)
            {
                TargetEnemy.Hurt(FinalDamage);
            }
            else
            {
                
                PlayRandomSounds(ResourceSystem.Instance.GetEnemy(TargetEnemy.type).DeathSounds);
                TargetEnemy.OnDisAppear();
                StartChew();
            }
        }
    }
    public void StartChew()
    {
                Particle2.SetActive(true);
                _animator.SetBool("IsChew", true);
                _animator.SetBool("IsChewOver", false);
                Defence = 20;
                MaybeUseBool = true;
                BloodParticle.SetActive(true);
    }
    public void EndChew()
    {
        Particle2.SetActive(false);
        _animator.SetBool("IsChew", false);
        _animator.SetBool("IsChewOver", true);
        MaybeUseTimer = 0;
        Defence = 0;
        MaybeUseBool = false;
        BloodParticle.SetActive(false);
        
    }
  

}
