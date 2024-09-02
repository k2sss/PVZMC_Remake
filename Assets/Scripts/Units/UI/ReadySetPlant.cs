using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySetPlant : Pannel
{
    public AudioClip clip;

    private void Start()
    {
        SoundSystem.Instance.Play2Dsound(clip);
    }
    private void Destroyy()
    {
        UIMgr.Instance.CloseUI("RSP");
    }
}
