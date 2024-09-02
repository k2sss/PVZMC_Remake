using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : BaseManager<LevelManager>
{
    public bool IsBossFight;
    public Mob Boss;
    public int nowWave = 0;
    public int MaxWave = 10;
    public int[] HugeWavesArray;
    public float WaveTimerInterval = 45;
    public float WaveTimer;
    public bool isStopTimer;
    public float FirstZombieAppearTime = 20;

    private float lastWaveTimer;
    private float NextWaveTimer = 2;

    public bool IsWin { get; private set; }

    private void Start()
    {
        //箱子逻辑
        EventMgr.Instance.AddEventListener("PickUpChest", () => MusicMgr.Instance.PlayMusic("WinMusic",0,true));
        EventMgr.Instance.AddEventListener("PickUpChest", MusicMgr.Instance.StopMusicLoop);

        EventMgr.Instance.AddEventListener("StartSelectCard", () => isStopTimer = true);
        EventMgr.Instance.AddEventListener("GameStart", () => isStopTimer = false);
        EventMgr.Instance.AddEventListener("GameOver", () => InputMgr.Instance.UnableAllInput());

        AudioClip[] clips = SoundSystem.Instance.GetAudioClips("HugeWave");
        //无读取存档时
        if (!MySystem.CanLoadLevelData())
        {
            EventMgr.Instance.AddEventListener("GameStart", () => Invoke("EnterFirstLevel", FirstZombieAppearTime));
            EventMgr.Instance.AddEventListener("GameStart", () => MonoController.Instance.Invoke(FirstZombieAppearTime, () => SoundSystem.Instance.Play2Dsound(clips[2])));
            MusicMgr.Instance.PlayMusic("SelectCard",0,true);
        }




        if (IsBossFight == true)
        {
            Boss.onMobDead += WinTheGame;
            Boss.onMobDead += CleanTheEnemy;
        }
    }

    private void Update()
    {
        NextWaveTimer -= Time.deltaTime;

        if (IsBossFight == false)
        {
            TimeCounter();
            CheckIsWin();
        }
        else
        {
            TimeCounterWhenBossFight();
        }
    }
    public void Stop()
    {
        isStopTimer = true;
    }

    private void CheckIsWin()
    {
        if (IsWin) return;
        if (nowWave != MaxWave) return;
        if (!IsPreparedForNextWave()) return;

       
        lastWaveTimer += Time.deltaTime;
        if (lastWaveTimer > 120)
        {
            WinTheGame();
            return;
        }
        if (!EnemyManager.Instance.CheckEnemys())
        {
            WinTheGame();
            return;
        }

    }

    private void TimeCounter()
    {
       
        if (!IsPreparedForNextWave()) return;
        if (isStopTimer) return;

        //每隔45s进入下一波
        WaveTimer += Time.deltaTime;
        if (WaveTimer > WaveTimerInterval)
        {
            EnterNextWave();
            WaveTimer = 0;
        }
        //如果场内的怪物已经清空
        else if (nowWave != 0 && !EnemyManager.Instance.CheckEnemys())
        {
            EnterNextWave();
            WaveTimer = 0;
        }

    }
    private void TimeCounterWhenBossFight()
    {
        if (!IsPreparedForNextWave()) return;
        if (isStopTimer || IsWin) return;

        WaveTimer += Time.deltaTime;

        if (WaveTimer > WaveTimerInterval / 2)
        {
            EnterNextWaveWhenBossFight();
            WaveTimer = 0;
        }
        else if (nowWave != 0 && !EnemyManager.Instance.CheckEnemys())
        {
            EnterNextWaveWhenBossFight();
            WaveTimer = 0;
        }


    }
    private bool IsPreparedForNextWave()
    {
        return NextWaveTimer < 0 ? true : false;
    }
    public void EnterFirstLevel()
    {
        if (nowWave != 0) return;

        EnterNextWave();
        WaveTimer = 0;
        
    }
    public void EnterNextWave()//进入下一波
    {

        if (nowWave >= MaxWave) return;
       
        
        if (nowWave < MaxWave)
        {
           
            nowWave += 1;
            NextWaveTimer = 2;
            EventMgr.Instance.EventTrigger("ChangeWave");
            EnemyManager.Instance.CreateNextWaveEnemys();
            
        }

        if (nowWave == MaxWave)
        {
            EventMgr.Instance.EventTrigger("LastWave");
        }
        else
        if (CompareBigWave(nowWave))
        {
            EventMgr.Instance.EventTrigger("HugeWave");
        }

    }

    public void EnterNextWaveWhenBossFight()//进入下一波
    {
       
        nowWave++;
        EventMgr.Instance.EventTrigger("ChangeWave");
        EnemyManager.Instance.CreateNextWaveEnemys();
        NextWaveTimer = 2;
    }


    public bool CompareBigWave(int wave)//检查是否为BigWave
    {

        if (HugeWavesArray == null)
            return false;

        for (int i = 0; i < HugeWavesArray.Length; i++)
        {
            if (wave == HugeWavesArray[i])
            {
                return true;
            }
        }
        return false;

    }
    private void WinTheGame()
    {
        if (IsWin == false)
        {
            IsWin = true;
            EventMgr.Instance.EventTrigger("GameWin");
        }
    }
    private void CleanTheEnemy()
    {
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Enemys.Length; i++)
        {
            Enemys[i].GetComponent<Enemy>().Hurt(1000);
        }
    }



}
