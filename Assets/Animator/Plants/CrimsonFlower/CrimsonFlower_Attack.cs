using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrimsonFlower_Attack : MonoBehaviour
{
    public float AttackDamage;
    private CrimsonFlower owner;

    public void Init(float Damage,CrimsonFlower owner)
    {
        AttackDamage = Damage;
        this.owner = owner;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            e.Hurt(AttackDamage);
            owner.GetTreatment(1f);
            e.Vertigo(0.2f);
            e.AddForce(new Vector3(3, 0, 0));
        }
    }
    
    public void EnterPool()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}
