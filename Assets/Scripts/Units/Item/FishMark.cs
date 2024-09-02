using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FishMark : MonoBehaviour
{
    public Transform target;
    public bool IsCatched;
    public Rigidbody rb;
    public bool isFishing { get; private set; }//是否处于钓鱼状态，如果处于钓鱼状态，则不会抓取怪物
    public Water targetWater { get; private set; }//目标水池
    private Vector3 FishingPos;
    private bool isFishingCatched;
    private int FishingPower = 15;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("FishingTimer", 3, 1);
    }
    private void Update()
    {
        if(isFishing)
        {
            //如果正处于钓鱼状态

            transform.position = FishingPos +0.3f* Vector3.up * Mathf.Sin(Time.time);
            rb.useGravity = false;
            return;
        }
        if(IsCatched == false)
        {
            Catch();
            rb.useGravity = true;
        }
        else if(target != null)
        {
            transform.position = target.position + new Vector3(0,1,0); 
            rb.useGravity = false;
        }
        
    }
    private void FishingTimer()
    {
        
        if (!InventoryManager.Instance.ThereIsItem(ItemType.鱼饵))
            return;
       
        if (isFishingCatched) isFishingCatched = false;
        else if(Random.Range(0,100)< FishingPower)
        {
            targetWater.CreateWaterParticle(transform.position);
            SoundSystem.Instance.PlayRandom2Dsound("Swim");
            isFishingCatched = true;
        }

    }
    void Catch()
    {
      
        target = FindTheNearestMob("Enemy", 2.5f).transform;
        if(target != null)
        {
            target.GetComponent<Enemy>().Hurt(1);
            IsCatched = true;
        }    

    }
    public void GetFish()
    {
        
        if (targetWater == null||!isFishingCatched)
            return;
        ItemType type = targetWater.FishingAward();
        InventoryManager.Instance.DeleteTargetItem(ItemType.鱼饵);
       // Debug.Log(type);   
        GameObject itemObj = Instantiate(ResourceSystem.Instance.GetItem(type).prefab);
        itemObj.transform.position = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1f), Random.Range(-0.3f, 0.3f));
        itemObj.transform.DOMove(PlayerHealth.instance.transform.position + Vector3.up,1);

    }
    public void Cancle(bool pull)
    {
        //将敌人拉回
        if (target != null&&pull == true)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            enemy.Hurt(1);
            enemy.Vertigo(0.3f);
            Vector3 dir = GameObject.FindGameObjectWithTag("Player").transform.position - target.transform.position;
            enemy.AddForce(dir.normalized * 5);
        }

        GetFish();
        isFishing = false;
        targetWater = null;
        IsCatched = false;
        rb.useGravity = true;
        target = null;
       
    }
    public GameObject FindTheNearestMob(string tag, float Range)
    {
        GameObject[] allmobs = GameObject.FindGameObjectsWithTag(tag);
        GameObject go = null;
        float mindistance = Mathf.Infinity;
        foreach (GameObject a in allmobs)
        {
            //比较大小
            float length = (transform.position - a.transform.position).magnitude;
            if (length < mindistance && length <= Range)
            {
                mindistance = length;
                go = a;
            }
        }
        return go;
    }
    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log("Obj:"+other.name);
        if (IsCatched||isFishing)
            return;
        if(other.CompareTag("Water"))
        {
          
            //Debug.Log("FindWater");
            FishingPos = transform.position;
            isFishing = true;
            targetWater = other.GetComponent<Water>();
            targetWater.CreateWaterParticle(transform.position); 
            SoundSystem.Instance.PlayRandom2Dsound("Swim");
        }
    }
    public void SetFishingPower(int power)
    {
        if(power < 0 && power > 100)
        {
            return;
        }
        FishingPower = power;

    }
    
}
