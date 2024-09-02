using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHugeWave : MonoBehaviour
{
    private Animator m_animator;
    private Image m_image;
    
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_image = GetComponent<Image>();
        m_image.enabled = false;
        EventMgr.Instance.AddEventListener("HugeWave", EnterHugeWave);
    }

    public void EnterHugeWave()
    {
        AudioClip[] clips = SoundSystem.Instance.GetAudioClips("HugeWave");
        SoundSystem.Instance.Play2Dsound(clips[0]);
        MonoController.Instance.Invoke(2, ()=>SoundSystem.Instance.Play2Dsound(clips[3]));
        m_image.enabled = true;
        m_animator.SetBool("Enter",true);
        Invoke("Quit", 3);
    }

    private void Quit()
    {
        m_animator.SetBool("Enter", false);
    }
    public void DisAppear()
    {
        m_image.enabled = false;
    }
}
