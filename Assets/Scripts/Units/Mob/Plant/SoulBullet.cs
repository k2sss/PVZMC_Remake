using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBullet : PeaBullet
{

    protected override void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 8)
        {
            Mob mob = other.GetComponent<Mob>();
            mob.AddBuff(BuffType.Vulnerable, 2.5f);
            mob.Hurt(Damage);
            InsanitiateParticle(direction);
            PlayRandomSound(hitsounds, SoundValue); 
            if (Random.Range(0, 100) < 50)
            {
                ObjectPool.Instance.PushObject(gameObject);
            }
        }

    }

}

