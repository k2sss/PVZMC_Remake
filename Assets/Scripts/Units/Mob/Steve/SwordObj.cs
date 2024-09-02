using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordObj : MonoBehaviour
{
    public float Damage;
    public void Init(float Damage,BuffType type,float timer,IAttacker attcker)
    {
        SwordLight s = transform.GetChild(0).gameObject.GetComponent<SwordLight>();
        s.SetDamage(Damage,attcker);
        s.AddBuff(type, timer, true);
    }
    public void Init(float Damage,IAttacker attcker)
    {
        
        SwordLight s = transform.GetChild(0).gameObject.GetComponent<SwordLight>();
       
        s.SetDamage(Damage,attcker);
       
        s.AddBuff(BuffType.DryDamage, 0, false);
    }
    private void Start()
    {
        
    }
    public void EnterPool()
    {
        Destroy(gameObject);
    }
}
