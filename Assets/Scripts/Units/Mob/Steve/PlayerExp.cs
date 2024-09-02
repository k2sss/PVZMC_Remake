using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerExp : BaseManager<PlayerExp>
{
    public int Exp;
    public int MaxExp;
    public int Level;
    public int a;
    public Action OnExpAdd;
    private AudioClip[] Sound;
    private void Start()
    {  
       Sound = SoundSystem.Instance.GetAudioClips("Exp");
    }
    public void AddExp(int value)
    {
        Exp += value;
        if (Exp > MaxExp)
        {    Exp = Exp - MaxExp;
            MaxExp += a;
            
            Level += 1;
            SoundSystem.Instance.Play2Dsound((Sound[1]));
        }
        OnExpAdd?.Invoke();
        SoundSystem.Instance.Play2Dsound((Sound[0]));
    }
    public void SubLevel(int i)
    {
        Level -= i;
        AddExp(0);
    }
    
}
