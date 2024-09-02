using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class HpBarMgr : BaseManager<HpBarMgr>
{
    private GameObject HpBarprefab;
    private bool Initialized;
    public bool isEnable;
    private void Start()
    {
        if (Initialized == false)
            Init();
    }
    public void Init()
    {
        HpBarprefab = FileLoadSystem.ResourcesLoad<GameObject>("UI/Terraria/HpBar");
        Initialized = true;
    }
    public void Create(Enemy enemy)
    {
      
        if (isEnable == true)
        {
           
            if (Initialized == false)
                Init();
            GameObject n = ObjectPool.Instance.GetObject(HpBarprefab);
            n.transform.SetParent(transform);
            TerraHPbar bar = n.GetComponent<TerraHPbar>();
            n.transform.localScale = Vector3.one;
            bar.SetTarget(enemy);
        }
    }
    public void Clean()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
             Destroy(transform.GetChild(i).gameObject);
        }
    }

}
