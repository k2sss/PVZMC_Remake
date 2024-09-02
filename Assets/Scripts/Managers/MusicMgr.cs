using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]
public class MusicMgr : BaseManager<MusicMgr>
{
    private AudioSource m_AudioSource;
    private bool isLock;
    public bool IsLock {
        get { 
            return isLock; 
        }
        set
        {
            isLock = value;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        m_AudioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Start()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "MainTitle":
                PlayMusic("MoogCity2");
                break;
            case "Store":
            case "InGameMenum":
            case "InGameMenu_1":
                PlayMusic("Minecraft");
                break;
            case "InGameMenu_2":
                PlayMusic("BloodCrawlers");
                break;
        };
    }
    public void PlayMusic(string MusicName, float time = 0f,bool isIgnoreLock = false)
    {
        if (isLock && !isIgnoreLock) return;

        if (SoundSystem.Instance.ContainMusic(MusicName))
        {
            m_AudioSource.clip = SoundSystem.Instance.GetMusicClip(MusicName);
            // m_AudioSource.PlayScheduled(time);
            m_AudioSource.Play();
        }
    }
    public void StopMusicLoop()
    {
        m_AudioSource.loop = false;
    }
    public void PauseMusic()
    {
        m_AudioSource.Pause();
    }
    public void GoOnMusic()
    {
        m_AudioSource.Play();
    }
}
