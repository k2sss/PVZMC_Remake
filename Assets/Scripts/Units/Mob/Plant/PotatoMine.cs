using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoMine : Plants
{
    private float Light_Timer;
    public float LightBreathTime;
    private bool IsLight;
    private bool IsGrowUp;
    public float GrowTime;
    public float Damage;
    public GameObject NearestEnemy;
    public AudioClip grow;
    public AudioClip boomSound;
    private bool isBoomed;
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        LightBreath();
        MaybeUseTimer += Time.deltaTime;
        if (MaybeUseTimer > GrowTime && IsGrowUp == false)
        {
            IsGrowUp = true;
            Grow();
        }
        NearestEnemy = FindTheNearestMob("Enemy", 1.5f, 1);
        if (NearestEnemy != null)
        {
            if (IsGrowUp == true)
                Detect();
        }
    }
    private void LightBreath()
    {
        Light_Timer += Time.deltaTime;
        if (Light_Timer > LightBreathTime)
        {
            Light_Timer = 0;
            //改变灯的颜色
            if (IsLight == false)
            {
                IsLight = true;
                renderers[0].material.SetColor("_EmissionColor", Color.white);
            }
            else
            {
                IsLight = false;
                renderers[0].material.SetColor("_EmissionColor", Color.black);
            }
        }
    }
    public void Grow()
    {
        _animator.SetBool("Grow", true);
        AudioSource s = SoundSystem.Instance.InitAudioSource(gameObject, SoundSystem.Instance.SoundMixer);
        s.PlayOneShot(grow);
        GameObject p = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.dirt));
        p.transform.position = transform.position;
    }
    public void Detect()
    {
        _animator.SetBool("Detect", true);

    }
    public void Boom()//由动画机调用
    {
        GameObject[] findenmeys = FindTheNearestMobs("Enemy", 1.9f);
        for (int i = 0; i < findenmeys.Length; i++)
        {
            findenmeys[i].GetComponent<Mob>().Hurt(Damage);
        }
        //粒子特效
        GameObject explodeP = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.boom));
        GameObject smokeP = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.smoke_burst));
        Vector3 offset = new Vector3(0, 0.4f, 0);
        explodeP.transform.position = transform.position + offset;
        AudioSource s = SoundSystem.Instance.InitAudioSource(explodeP, SoundSystem.Instance.SoundMixer);
        s.PlayOneShot(boomSound);
        smokeP.transform.position = transform.position + offset;
        CameraAction.Instance.StartShake();
        isBoomed = true;
        Death();
        Destroy(gameObject);
    }
    public override void Death()
    {
        if (IsGrowUp == true&&isBoomed == false)
        {
            GameObject[] findenmeys = FindTheNearestMobs("Enemy", 1.5f);
            for (int i = 0; i < findenmeys.Length; i++)
            {
                findenmeys[i].GetComponent<Mob>().Hurt(Damage);
            }
            //粒子特效
            GameObject explodeP = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.boom));
            GameObject smokeP = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.smoke_burst));
            Vector3 offset = new Vector3(0, 0.4f, 0);
            explodeP.transform.position = transform.position + offset;
            AudioSource s = SoundSystem.Instance.InitAudioSource(explodeP, SoundSystem.Instance.SoundMixer);
            s.PlayOneShot(boomSound);
            smokeP.transform.position = transform.position + offset;
            CameraAction.Instance.StartShake();
            isBoomed = true;
        }

        base.Death();
    }

}
