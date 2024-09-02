using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : Plants
{
    public GameObject Particle;
    public AudioClip[] Sound;
    public GameObject explodeBox;
    private AttackBox attackBox;
    protected override void Start()
    {
        base.Start();
        PlayOneShot(Sound[0]);
        explodeBox.SetActive(false);
        attackBox = explodeBox.GetComponent<AttackBox>();
        attackBox.triggerEnterEvent += (Collider other) =>
        {
            if (other.gameObject.CompareTag("Grid"))
            {
                Grid g = other.GetComponent<Grid>();
                    if (g.gridType == GridType.blood)
                    {
                        g.Transfer1();
                    }
            }
        };
        
    }

    public void AttackBoxOn()
    {
        explodeBox.SetActive(true);
    }
    public void Explode()
    {
        //��Ч:

        GameObject p = ObjectPool.Instance.GetObject(Particle);
        p.transform.position = transform.position;
        CameraAction.Instance.StartShake();
        //��Ч��
        AudioSource audioS = SoundSystem.Instance.InitAudioSource(p, SoundSystem.Instance.SoundMixer);
        audioS.PlayOneShot(Sound[1]);
        //ʵ������

        Death();

        MonoController.Instance.Invoke(0.1f,WorldManager.UpdateWorldInfo);


    }
}
public interface Ipurefiable
{
    public void OnPurify();
    public void OnEndPurify();
}