using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBox : MonoBehaviour
{
    public float PlayerDamage;
    public float EnemyDamage;
    public float PlantDamage;
    public float blockDamage;
    private AudioSource source;
    private float explodeTimer;
    private BoxCollider Boxcollider;
    private AudioClip[] clips;
    public void InitDamage(float whenplayer, float whenzombie, float whenplants, float blockdamage)
    {
        this.PlayerDamage = whenplayer;
        this.EnemyDamage = whenzombie;
        this.PlantDamage = whenplants;
        this.blockDamage = blockdamage;
        explodeTimer = 0;
        source = SoundSystem.Instance.InitAudioSource(gameObject, SoundSystem.Instance.SoundMixer);
        clips = SoundSystem.Instance.GetAudioClips("Explode");
        source.PlayOneShot(clips[Random.Range(0,clips.Length)]);
       
    }
    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        Boxcollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        explodeTimer += Time.deltaTime;
        if (explodeTimer > 0.2f)
        {
            Boxcollider.enabled = false;
        }
        else
        {
            Boxcollider.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 9)
        {
            if (other.CompareTag("Player"))
            {
               
                other.GetComponent<Mob>().Hurt(PlayerDamage);
              
            }
            else
            {
              
                    other.GetComponent<Mob>().Hurt(PlantDamage);
               
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Mob>().Hurt(EnemyDamage);
            }
            if (other.CompareTag("Blocks"))
            {
                other.GetComponent<FunctionalBlock>().CauseDamage(blockDamage, BlockStrengthType.normal, 0);
               
            }
        }



    }

}
