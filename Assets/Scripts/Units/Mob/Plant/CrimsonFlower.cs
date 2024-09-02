using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonFlower : Plants
{
    private GameObject bullet;
    public AudioClip clip;
    private float TreatTimer;

    protected override void Start()
    {
        base.Start();
        bullet = FileLoadSystem.ResourcesLoad<GameObject>("bullet/Red");
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        Check();
        TreatTimer += Time.deltaTime;
    }
    public void Check()
    {
        if (MyPhysics.BoxRayCheck<Mob>(transform.position + new Vector3(-0.2f, 0.5f, 0), 0.5f, new Vector3(1, 0, 0), 5, 8) != null)
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
    public void Attack()
    {
        CrimsonFlower_Attack a = ObjectPool.Instance.GetObject(bullet).GetComponent<CrimsonFlower_Attack>();
        a.transform.position = transform.position + new Vector3(3, 1, 0);
        a.Init(FinalDamage, this);
        SoundSystem.Instance.Play2Dsound(clip, 0.3f);
    }
    public override void GetTreatment(float value,bool isShowingEffects = true)
    {
        if (Health < MaxHealth)
        {
            if (TreatTimer > 0)
            {
                GameObject p = ObjectPool.Instance.GetObject(ResourceSystem.Instance.GetParticle(ParticleType.Glint));
                p.transform.position = transform.position;
                TreatTimer = -1;
            }
            if (Health + value <= MaxHealth)
            {
                Health += value;
            }
            else
            {
                Health = MaxHealth;
            }


            ChangeMaterialColor(new Vector4(0.3f, 1.4f, 0.3f, 1));
            MonoController.Instance.Invoke(0.3f, () => ChangeMaterialColor(Color.white));
        }


    }
}
