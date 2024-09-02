using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBucket : MonoBehaviour
{
    [SerializeField]private float Damage = 50;
    public GameObject explodeBox;
    public void Explode()
    {
        if (explodeBox == null)
            return;

        GameObject box =  Instantiate(explodeBox);
        box.transform.position = transform.position;
        ExplodeBox b = box.GetComponent<ExplodeBox>();
        b.InitDamage(5, Damage, 5, Damage);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            CameraAction.Instance.StartShake();
            Explode();
            Destroy(gameObject);
        }
    }
}
