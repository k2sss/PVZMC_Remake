using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlower : Plants, IproduceSun
{
    public float SunCreateGapTimer;
    public float StartSpeed;
    public AudioClip[] createSunClips;
    private float effectiveTimer;//激化时间

    protected override void Start()
    {
        base.Start();
        if (MaybeUseBool == false)//判断是否已经加过一次时长
        {
            MaybeUseTimer += 9;
            MaybeUseBool = true;
        }
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
        CreateSunTimer();
        effectiveTimer -= Time.deltaTime;
    }
    private void CreateSunTimer()
    {
        if (Disable == false)
        {
            MaybeUseTimer += Time.deltaTime * AttackSpeedMultiplier;

            if (effectiveTimer > 0)
            {
                MaybeUseTimer += 0.25f * Time.deltaTime;
            }

        }

        if (MaybeUseTimer > SunCreateGapTimer)
        {
            MaybeUseTimer = 0;
            _animator.SetBool("CreateSun", true);
        }
    }
    public void CreateSun()
    {
        GameObject s = ObjectPool.Instance.GetObject(SunManager.Instance.sunPrefab);
        Sun sun = s.GetComponent<Sun>();
        sun.init();
        sun.rb.velocity = new Vector3(0, StartSpeed, 0);
        s.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 2, Random.Range(-0.5f, 0.5f));
        PlayRandomSounds(createSunClips);
    }
    public void CreateSunAnimatorFalse()
    {
        _animator.SetBool("CreateSun", false);
    }

    void IproduceSun.ProductivityUp()
    {
        effectiveTimer = 5;
    }
}
