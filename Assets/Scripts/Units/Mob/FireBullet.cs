using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    private AudioClip firepea;
    public AudioSource source;
    private void Start()
    {
        firepea = SoundSystem.Instance.GetAudioClips("FirePea")[0];
        source = SoundSystem.Instance.InitAudioSource(gameObject,SoundSystem.Instance.SoundMixer);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector3(BulletSpeed, 0, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == 8)
        {
            Mob mob = other.GetComponent<Mob>();

            mob.Hurt(Damage);

            source.PlayOneShot(firepea);
            //InsanitiateParticle(direction);
            //PlayRandomSound(hitsounds);
        }
        else if (other.gameObject.layer == 3)
        {
            if (other.GetComponent<FunctionalBlock>() != null)
            {
                other.GetComponent<FunctionalBlock>().CauseDamage(Damage, BlockStrengthType.normal, 0, 1, 0.5f);
            }
            //InsanitiateParticle(direction);
            //PlayRandomSound(hitsounds);

        }
    }

}
