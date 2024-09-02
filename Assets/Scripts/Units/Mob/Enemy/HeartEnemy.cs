using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartEnemy : Enemy
{
    

    protected override void Start()
    {
        base.Start();
        GenerateHpBar();

    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage, penetration, Dtype);
        ShowDamage(Damage);
    }
    public override void Death()
    {
        base.Death();
        OnDisAppear();
    }
    public override void OnDisAppear()
    {
        EventMgr.Instance.EventTrigger("HEART");
        SoundSystem.Instance.PlayRandom2Dsound(ResourceSystem.Instance.GetEnemy(type).DeathSounds);
        base.OnDisAppear();
    }
}
