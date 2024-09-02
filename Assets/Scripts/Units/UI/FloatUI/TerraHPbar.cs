using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerraHPbar : MonoBehaviour
{
    private Image hpfill;
    private Image hpbar;
    private Enemy targetMonster;

    private float hpfillwidth;
   
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        hpbar = GetComponent<Image>();
        hpfill = transform.GetChild(0).GetComponent<Image>();
        hpfillwidth = hpfill.rectTransform.sizeDelta.x;
    }
    public void SetTarget(Enemy Target)
    {
        targetMonster = Target;


        Target.onMobDisapear += () =>
        {
            ObjectPool.Instance.PushObject(gameObject);
        };
        Target.onMobHealthChange += () =>
        {
            InfoUpdate();
        };
        InfoUpdate();
    }
    private void Update()
    {
        PositionUpdate();
    }
    private void PositionUpdate()
    {
        if (targetMonster == null)
            return;

        transform.position = Camera.main.WorldToScreenPoint(targetMonster.transform.position);
    }
    private void InfoUpdate()
    {
        if (HpBarMgr.Instance.isEnable == false)
        {
            hpfill.color = Vector4.zero;
            hpbar.color = Vector4.zero;
            return;
        }

        //如果满血不显示

        
        float value = (targetMonster.Health / targetMonster.MaxHealth);
        hpfill.rectTransform.sizeDelta = new Vector2(hpfillwidth * value, hpfill.rectTransform.sizeDelta.y);


        if (value >= 1f)
        {
            hpfill.color = Vector4.zero;
            hpbar.color = Vector4.zero;
        }
        else if (value > 0.5f)
        {
            hpfill.color = new Vector4((1 - value) * 2, 1f, 0f, value * 0.6f + 0.1f);
            hpbar.color = new Vector4(1, 1, 1, value * 0.7f + 0.1f);
        }
        else
        {
            hpfill.color = new Vector4(1f, value * 2, 0f, value * 0.6f + 0.1f);
            hpbar.color = new Vector4(1, 1, 1, value * 0.7f + 0.1f);
        }



    }
}
