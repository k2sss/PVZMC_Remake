using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioResources 
{
    public List<SingleAudio> list = new List<SingleAudio>();

   public IEnumerator GetEnumerator()
    {
        for(int i = 0;i<list.Count;i++)
        {
            yield return list[i];
        }
    }
}
[System.Serializable]
public class SingleAudio
{
    public string clipName;
    public AudioClip[] clips;
}