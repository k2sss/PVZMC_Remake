using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : Enemy
{
    public Enemy owner;
    private AudioClip[] clips;
    public Sprite breakSprite;
    public Animator sheildAnimator;
    //public Material defaultMAt;
    public Material breakMat;
    protected override void Awake()
    {
        _collider = GetComponent<Collider>();
        gameObject.layer = 8;
        gameObject.tag = "Enemy";
       
    }
    protected override void Start()
    {
        base.Start(); 
        clips = SoundSystem.Instance.GetAudioClips("Wood");
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        // Debug.Log("114514");
        owner.ArmorHealth -= Damage;
        if(owner.ArmorHealth < 100)
        {
            ChangeMaterial(breakMat);
        }

        if (owner.ArmorHealth <= 0)
            Break();
        ChangeMaterialColor(HurtColor);
        Invoke("AfterHurt", HurtHoldTime);
        _audioSource.PlayOneShot(clips[Random.Range(0,clips.Length)]);
        sheildAnimator.SetBool("OnHit", true);
        MonoController.Instance.Invoke(0.3f, () => sheildAnimator.SetBool("OnHit", false));
    }
    public void Break()
    {
        gameObject.SetActive(false);
        SoundSystem.Instance.Play2Dsound(clips[Random.Range(0, clips.Length)]);
        GameObject breakParticle = ObjectPool.Instance.GetObject(ResourceSystem.Instance.GetParticle(ParticleType.break_particles_small));
        breakParticle.transform.position = transform.position;
        breakParticle.GetComponent<ParticleSlice>().CutedSprite = breakSprite;
        breakParticle.GetComponent<ParticleSystem>().Emit(20);
    }
    public override void Vertigo(float time)
    {
        
    }
    public override void AddForce(Vector3 dir)
    {
       
    }
}
