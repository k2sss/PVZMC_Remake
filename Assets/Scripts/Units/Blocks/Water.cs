using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Water : MonoBehaviour
{
    private GameObject waterParticle;
    private GameObject waterParticle1;
    public bool isflowWater;
    public float forceStrength = 4;
    public GameObject arrow;
    public Renderer[] renders;
    public bool isBlood;
    private float particleTimer;
    private WaterPoolList normalWater;
    private WaterPoolList bloodWater;

    private void Start()
    {
        waterParticle = FileLoadSystem.ResourcesLoad<GameObject>("Particles/WaterParticle");
        waterParticle1= FileLoadSystem.ResourcesLoad<GameObject>("Particles/WaterParticle1");
        normalWater = Resources.Load<WaterPoolList>("SciptableObjects/WaterPool/NormalPool");
        bloodWater = Resources.Load<WaterPoolList>("SciptableObjects/WaterPool/BloodWater");
        if (arrow != null)
        {
            arrow.SetActive(false);
        }
       
    }
    private void Update()
    {
        particleTimer += Time.deltaTime;
    }
    public void TurnPure()
    {
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].material.color = new Color(0, 0.6f, 1,1);

        }
        isBlood = false;
    }
    public void TurnBlood()
    {
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].material.color = new Color(1, 0.2f, 0, 1);
        }
        isBlood = true;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {

            Mob m = other.GetComponent<Mob>();
            m.WaterGridCount++;
            if (isflowWater == true)
            {
                m.WaterForce += forceStrength * -transform.forward;
            }
            if (m.WaterGridCount == 1)
            {
                m.IsInWater = true;
                m.SetNowSpeed(-0.4f);
                IWater iwater = m as IWater;

                GameObject pa;
                if(isBlood == false)
                pa = ObjectPool.Instance.GetObject(waterParticle);
                else
                pa = ObjectPool.Instance.GetObject(waterParticle1);
                pa.transform.position = m.transform.position;


                SoundSystem.Instance.PlayRandom2Dsound("Splash");
                if (iwater != null)
                {
                    iwater.IEnterWater();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {

            Mob m = other.GetComponent<Mob>();
            m.WaterGridCount--;
            if (isflowWater == true)
            {
                m.WaterForce -= forceStrength * -transform.forward;
            }
            if (m.WaterGridCount == 0)
            {
                m.SetNowSpeed(+0.4f);
                m.IsInWater = false;
                if (particleTimer > 0.2f)
                {

                 CreateWaterParticle(m.transform.position);
                 SoundSystem.Instance.PlayRandom2Dsound("Swim");
                 particleTimer = 0;
                }
             
                m.WaterForce = Vector3.zero;
                IWater iwater = m as IWater;
                if (iwater != null)
                {
                    iwater.IExitWater();
                }
            }
        }
    }

    public void CreateWaterParticle(Vector3 pos)
    {
        GameObject pa;
        if (isBlood == false)
            pa = ObjectPool.Instance.GetObject(waterParticle);
        else
            pa = ObjectPool.Instance.GetObject(waterParticle1);
        pa.transform.position = pos;
    }
    public ItemType FishingAward()
    {
        WaterPoolList target;

        if (!isBlood)
            target = normalWater;
        else
            target = bloodWater;

        int totalWeight = target.poolObj.Select(n => n.weight).Sum();
        
        int randN = Random.Range(0, totalWeight);

        for(int i = 0;i<target.poolObj.Length;i++)
        {
            int weight = target.poolObj[i].weight;

            if(randN < weight)
            {
                return (ItemType)target.poolObj[i].ItemId;
            }    
            else
            {
                randN -= weight;
            }
        }
      
        return ItemType.Nothing;
    }
    
}

public interface IWater
{
    public void IEnterWater();
    public void IExitWater();
}
