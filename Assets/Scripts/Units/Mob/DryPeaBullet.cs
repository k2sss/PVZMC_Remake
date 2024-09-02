using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryPeaBullet : PeaBullet
{

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Mob mob = other.GetComponent<Mob>();
            mob.Hurt(Damage);
            mob.AddBuff(BuffType.DryDamage, 1);
            InsanitiateParticle(direction);
            PlayRandomSound(hitsounds);
            ObjectPool.Instance.PushObject(gameObject);
        }
        else if (MyPhysics.CheckBox(transform.position, 0.01f, 3))
        {
            if (other.GetComponent<FunctionalBlock>() != null)
            {
                other.GetComponent<FunctionalBlock>().CauseDamage(Damage, BlockStrengthType.normal, 0, 1, 0.5f);
            }

            InsanitiateParticle(direction);
            PlayRandomSound(hitsounds);
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
