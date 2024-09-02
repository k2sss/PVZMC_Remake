using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : BaseManager<SoundSystem>
{
    private AudioSource audioSource;
    public AudioMixerGroup totalMixer;
    public AudioMixerGroup MusicMixer;
    public AudioMixerGroup SoundMixer;
    public AudioResources resources;
    public AudioResources musicResources;
    public EnemyIdleSoundMgr idleMgr;
    private Dictionary<string, SingleAudio> audioDic = new Dictionary<string, SingleAudio>();

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        idleMgr = new EnemyIdleSoundMgr();


        foreach (SingleAudio item in resources)
        {
            audioDic.Add(item.clipName, item);
        }
    }
    private void Update()
    {
       
        idleMgr.Update();
    }
    public AudioSource InitAudioSource(GameObject target,AudioMixerGroup group,float blendValue = 1,AudioRolloffMode type =AudioRolloffMode.Custom,float maxDistance = 100)
    {
        AudioSource source;
        if (target.GetComponent<AudioSource>() != null)
        {
            source = target.GetComponent<AudioSource>();
        }
        else
        {
            source = target.AddComponent<AudioSource>();
        }
        
        source.outputAudioMixerGroup = group;
        source.spatialBlend = blendValue;//设置为完全3D音效
        source.rolloffMode = type;
        source.maxDistance = maxDistance;
        return source;
    }

    public void RigistEnemyIdleSounds(Enemy enemy,float lowestTime,float highestTime,float AudioValue = 0.5f,float gap = 3)
    {
        idleMgr.idleSounds.Add(new EnemyIdleSound(enemy, lowestTime, highestTime,AudioValue,gap));
    }

    public void Play2Dsound(AudioClip clip,float value = 1)
    {
        audioSource.PlayOneShot(clip,value);
    }
    public void Play2Dsound(string clipName, float value = 1)
    {
        if(clipName != "")
        audioSource.PlayOneShot(GetAudioClips(clipName)[0], value);
    }
    public void PlayRandom2Dsound(AudioClip[] clip, float value = 1)
    {
        audioSource.PlayOneShot(clip[Random.Range(0, clip.Length)],value);
    }
    public void PlayRandom2Dsound(string clipName, float value = 1)
    {
        AudioClip[] clips = GetAudioClips(clipName);
        audioSource.PlayOneShot(clips[Random.Range(0,clips.Length)], value);
    }
    public AudioClip[] GetAudioClips(string ClipName)
    {
     
        if(audioDic.ContainsKey(ClipName))
        return audioDic[ClipName].clips;

        Debug.Log("找不到Audio");
        return null;
    }

    public AudioClip GetMusicClip(string musicName)
    {
        for (int i = 0; i < musicResources.list.Count; i++)
        {
            if (musicResources.list[i].clipName == musicName)
            {
                return musicResources.list[i].clips[0];
            }
        }
        Debug.Log("找不到MusicResources");
        return null;
    }
    public bool ContainMusic(string musicName)
    {
        for (int i = 0; i < musicResources.list.Count; i++)
        {
            if (musicResources.list[i].clipName == musicName)
            {
                return true;
            }
        }
        return false;
    }
}
public class EnemyIdleSound
{
    public Enemy target;
    public EnemyType type;
    public float minTime;
    public float maxTime;
    public float timer;
    public float randTime;
    public float audiovalue;
    public float gap;

    public EnemyIdleSound(Enemy enemy,float minTime,float maxTime, float AudioValue = 0.5f, float gap = 3)
    {
       
        this.target = enemy;
        type = enemy.type;
        this.minTime = minTime;
        this.maxTime = maxTime;
        randTime = UnityEngine.Random.Range(minTime, maxTime);
        this.audiovalue = AudioValue;
        this.gap = gap;
    }
}
public class EnemyIdleSoundMgr
{
    public List<EnemyIdleSound> idleSounds = new List<EnemyIdleSound>();
    public Dictionary<EnemyType, float> timers = new Dictionary<EnemyType, float>();
    public void Update()
    {
        //判断
        List<EnemyType> keys = new List<EnemyType>(timers.Keys);
        for (int i = 0; i < timers.Count; i++)
        {
            timers[keys[i]] += Time.deltaTime;
        }

        for (int i = 0; i < idleSounds.Count; i++)
        {
            if (idleSounds[i].target == null)
            {
               
                idleSounds.RemoveAt(i);
            }
        }
        for (int i = 0; i < idleSounds.Count; i++)
        {
           
            idleSounds[i].timer += Time.deltaTime;

            if (idleSounds[i].timer > idleSounds[i].randTime)
            {
                idleSounds[i].timer = 0;
                idleSounds[i].randTime = UnityEngine.Random.Range(idleSounds[i].minTime, idleSounds[i].maxTime);
                //播放音效
                if (!timers.ContainsKey(idleSounds[i].type))
                {
                    timers.Add(idleSounds[i].type, 0);
                    SoundSystem.Instance.PlayRandom2Dsound(ResourceSystem.Instance.GetEnemy(idleSounds[i].type).IdleSounds, idleSounds[i].audiovalue);
                }
                
                else if (timers[idleSounds[i].type] > idleSounds[i].gap)
                {
                    timers[idleSounds[i].type] = 0;
                    SoundSystem.Instance.PlayRandom2Dsound(ResourceSystem.Instance.GetEnemy(idleSounds[i].type).IdleSounds, idleSounds[i].audiovalue);
                }
          

            }

            }



    }

}