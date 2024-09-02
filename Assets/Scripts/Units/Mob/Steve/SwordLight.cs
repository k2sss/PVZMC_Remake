using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLight : MonoBehaviour
{
    public float Damage { get; private set; }
    public GameObject parent;
    private BuffType bufftype;
    private float bufftimer;
    private bool isAddBuff;
    private IAttacker attacker;
    
    public void SetDamage(float Damage, IAttacker attcker)
    {
        this.Damage = Damage;
        this.attacker = attcker;
    }
    public void AddBuff(BuffType type,float timer,bool doit)
    {
        bufftype = type;
        bufftimer = timer;
        isAddBuff = doit;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
             Enemy e = other.GetComponent<Enemy>();

             AttackMgr.AttackWithBuff(attacker, e, Damage,bufftype,bufftimer);
        }
    }
 
}
