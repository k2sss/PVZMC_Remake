using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodThornAttack : MonoBehaviour
{


    private AudioClip clip;
    private GameObject particle;
    private GameObject attackBox;
    private void Start()
    {
        particle = transform.Find("AttackThornParticle").gameObject;
        attackBox = transform.Find("Thorn").gameObject;
        attackBox.SetActive(false);
        particle.SetActive(false);
        clip = Resources.Load<AudioClip>("Sound/Item_8");
    }
    private void ShowParticle()
    {

        SoundSystem.Instance.Play2Dsound(clip);
        particle.SetActive(true);
        attackBox.SetActive(true);
    }

    //Ïú»Ù
    private void Disapper()
    {
        Destroy(gameObject);
    }
}
