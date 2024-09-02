using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBullet : Bullet
{
    public float HitStrength = 1;
    public override void init(Vector3 direction, float Damage, float BulletSpeed, bool IsEnemyType = false)
    {
        base.init(direction, Damage, BulletSpeed);
        rb.velocity = direction.normalized * BulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Enemy mob = other.GetComponent<Enemy>();
            mob.Hurt(Damage);
            if (mob.GetComponent<Rigidbody>() != null)
            {
                mob.Vertigo(0.3f);
                mob.AddForce(new Vector3(HitStrength, 0, 0));
            }

            GameObject p = ObjectPool.Instance.GetObject(hitParticle);
            p.transform.right = Vector3.up;
            p.transform.position = transform.position;
            //PlayRandomSound(hitsounds);
            ObjectPool.Instance.PushObject(gameObject);
        }
        else if (other.gameObject.layer == 3)
        {
            if (other.GetComponent<FunctionalBlock>() != null)
            {
                other.GetComponent<FunctionalBlock>().CauseDamage(Damage, BlockStrengthType.normal, 0, 1, 0.5f);
            }
            ObjectPool.Instance.PushObject(gameObject);
            GameObject p = ObjectPool.Instance.GetObject(hitParticle);
            p.transform.rotation = Quaternion.identity;
            p.transform.position = transform.position;
            //PlayRandomSound(hitsounds);

        }
    }

}
