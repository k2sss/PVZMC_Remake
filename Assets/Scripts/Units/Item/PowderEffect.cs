using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowderEffect : MonoBehaviour
{

    private Collider thisCollider;
    public enum PowderType
    {
        Purify,
        Bloodfy,
    }
    [SerializeField] private PowderType type;
    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
    }
    private void Start()
    {
        MonoController.Instance.Invoke(0.2f, () => thisCollider.enabled = false);
         MonoController.Instance.Invoke(0.1f, WorldManager.UpdateWorldInfo);
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(type)
        {
            case PowderType.Purify:
                OnTriggerEnterWhenPurify(other);
                break;
            case PowderType.Bloodfy:
                OnTriggerEnterWhenBloodify(other);
                break;
        }
      

    }
    private void OnTriggerEnterWhenPurify(Collider other)
    {
        if (other.CompareTag("Grid"))
        {
            other.gameObject.GetComponent<Grid>().Transfer1();
        }
        else if(other.CompareTag("Enemy"))
        {
            Enemy p = other.gameObject.GetComponent<Enemy>();
            p.AddBuff(BuffType.purify,10);
        }
    }
    private void OnTriggerEnterWhenBloodify(Collider other)
    {
        if (other.CompareTag("Grid"))
        {
            other.gameObject.GetComponent<Grid>().Transfer2();
        }
        
    }
    

}
