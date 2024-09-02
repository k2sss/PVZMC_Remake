using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TimerSetActive))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    protected Rigidbody rb;
    protected float Damage;
    protected float BulletSpeed;
    public Vector3 direction;
    public GameObject hitParticle;
    public AudioClip[] hitsounds;
    public float SoundValue = 1;
    public bool IsEnemyType;//是不是敌方的子弹

    public virtual void init(Vector3 direction, float Damage, float BulletSpeed, bool IsEnemyType = false)//初始化
    {
        gameObject.tag = "Bullet";
        rb = GetComponent<Rigidbody>();
        this.direction = direction;
        this.BulletSpeed = BulletSpeed;
        this.Damage = Damage;
        this.IsEnemyType = IsEnemyType;
    }
    public virtual void InsanitiateParticle(Vector3 dir)
    {
        if (hitParticle != null)
        {
            GameObject p = ObjectPool.Instance.GetObject(hitParticle);
            p.transform.right = dir;
            p.transform.position = transform.position;
        }
    }

    public void PlayRandomSound(AudioClip[] clips,float SoundValue = 1)
    {
        if (clips.Length > 0)
            SoundSystem.Instance.Play2Dsound(clips[Random.Range(0, clips.Length)],SoundValue);
    }
    public virtual void OnHit(Collider other)
    {
        if (IsEnemyType == false)
        {
            if (other.gameObject.layer == 8)
            {
                Mob mob = other.GetComponent<Mob>();

                mob.Hurt(Damage);
                InsanitiateParticle(direction);
                PlayRandomSound(hitsounds, SoundValue);
                ObjectPool.Instance.PushObject(gameObject);
            }
            else if (other.gameObject.layer == 3)
            {
                if (other.GetComponent<FunctionalBlock>() != null)
                {
                    other.GetComponent<FunctionalBlock>().CauseDamage(Damage, BlockStrengthType.normal, 0, 1, 0.5f);
                }
                ObjectPool.Instance.PushObject(gameObject);
                InsanitiateParticle(direction);
                PlayRandomSound(hitsounds,SoundValue);
            }
        }
        else
        {
            if (other.gameObject.layer == 9)
            {
                Mob mob = other.GetComponent<Mob>();

                mob.Hurt(Damage);
                InsanitiateParticle(direction);
                PlayRandomSound(hitsounds);
                ObjectPool.Instance.PushObject(gameObject);
            }
            else if (other.gameObject.layer == 3)
            {
                if (other.GetComponent<FunctionalBlock>() != null)
                {
                    other.GetComponent<FunctionalBlock>().CauseDamage(Damage, BlockStrengthType.normal, 0, 1, 0.5f);
                }
                ObjectPool.Instance.PushObject(gameObject);
                InsanitiateParticle(direction);
                PlayRandomSound(hitsounds);
            }
        }

    }
}
