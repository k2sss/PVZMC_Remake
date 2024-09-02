using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCreeper_BulletType : Enemy
{
    
     
    protected override void Start()
    {
        base.Start();
        GenerateHpBar();
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        ShowDamage(Damage);
        base.Hurt(Damage, penetration, Dtype);
    }
    protected override void Update()
    {
        base.Update();
        if (target == null|| target.IsDead)
        {
            Death();
            return;
        }

        transform.position = Vector3.Slerp(transform.position, target.transform.position, 2 * Time.deltaTime);
    }
    public void Attack()
    {
        if (target == null) return;
        target.Hurt(2);
    }
    public void SetTarget(Mob target)
    {
        this.target = target;
    }
    public override void Death()
    {
        base.Death();
        SoundSystem.Instance.PlayRandom2Dsound(ResourceSystem.Instance.GetEnemy(type).DeathSounds);
        OnDisAppear();
    }
}
