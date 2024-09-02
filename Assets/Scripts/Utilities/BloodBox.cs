using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockBox : MonoBehaviour
{
    public float PlayerDamage;
    public float PlantDamage;
    private AudioSource source;
    private float explodeTimer;
    private BoxCollider Boxcollider;
    private AudioClip[] clips;
    private float BloodBuffTime;
    public void InitDamage(float whenplayer, float whenplants,float time)
    {
        this.PlayerDamage = whenplayer;
        this.PlantDamage = whenplants;
        explodeTimer = 0;
        BloodBuffTime = time;
        source = SoundSystem.Instance.InitAudioSource(gameObject, SoundSystem.Instance.SoundMixer);
        clips = SoundSystem.Instance.GetAudioClips("Explode");
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        

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
                other.GetComponent<Mob>().AddBuff(BuffType.CrimsonDecay,BloodBuffTime);
            }
            else
            {

                other.GetComponent<Mob>().Hurt(PlantDamage);
                other.GetComponent<Mob>().AddBuff(BuffType.CrimsonDecay,BloodBuffTime);
            }
        }
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<Grid>().Transfer2();
        }

    }

}