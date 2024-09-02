using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumMgr : BaseManager<DamageNumMgr>
{
    private bool Initialized;
    private GameObject numPrefab;
    public bool isEnable;
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if (!Initialized)
        {
         
            numPrefab = FileLoadSystem.ResourcesLoad<GameObject>("UI/Terraria/Num");
            Initialized = true;
        }
    }
    public void Create(Vector3 attackPlace, float Damage)
    {
        if (isEnable == true)
        {
            Init();
            int damage = (int)(Damage);
            if (damage < 0)
            {
                return;
            }
            DamageNum num = Instantiate(numPrefab, transform).GetComponent<DamageNum>();
            num.transform.position = Camera.main.WorldToScreenPoint(attackPlace + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)));
            num.transform.localScale *= Random.Range(0.8f, 1.5f);
            num.Init(damage);
        }

    }
}
