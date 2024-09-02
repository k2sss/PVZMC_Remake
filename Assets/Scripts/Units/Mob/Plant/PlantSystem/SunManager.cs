using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunManager : BaseManager<SunManager>
{
    public GameObject sunPrefab;
    public float FallOriginHeight = 20;
    public bool FallSun = true;
    public float SunCreateGapTime;
    [SerializeField]private float FallTimer;
    private bool TimerGo = false;
    private Vector3 pointOrigin;
    private float Width;
    private float Length;
    private void Start()
    {
        SetLimit();
      
        EventMgr.Instance.AddEventListener("GameStart", () => TimerGo = true);
        EventMgr.Instance.AddEventListener("GameWin", () => TimerGo = false);
    }
    private void Update()
    {
        if(FallSun == true)
        SunCreate();
    }
    private void SetLimit()//ÉèÖÃÑô¹âµôÂä·¶Î§
    {
        pointOrigin = GridManager.Instance.transform.position;
        Width = GridManager.Instance.width;
        Length = GridManager.Instance.length/2f;
    }
    private void SunCreate()
    {
        if(TimerGo == true)
        FallTimer += Time.deltaTime;
        if (FallTimer > SunCreateGapTime)
        {
            FallTimer = 0;
            InsaniateASun();
        }
    }

    private void InsaniateASun()
    {
        GameObject s = ObjectPool.Instance.GetObject(sunPrefab);
        Sun sun = s.GetComponent<Sun>();
        sun.init();
        float RandomX = Random.Range(pointOrigin.x,pointOrigin.x+Length);
        float RandomZ = Random.Range(pointOrigin.z, pointOrigin.z + Width);
        s.transform.position = new Vector3(RandomX, FallOriginHeight, RandomZ);
    }
    public void StartCreate()
    {
        TimerGo = true;
    }

    public void EndCreate()
    {
        TimerGo = false;
    }


}
