using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun2 : MonoBehaviour
{
    public Transform TargetPos;
    public float damage;
    public float radius;
    public float moveSpeed;
    public float rotateSpeed;
    private bool isgoaway;
    private Vector3 preDir;
    public void Init(float Damage,float Radius,float MoveSpeed,float RotateSpeed,Transform target)
    {
        this.TargetPos = target;
        this.damage = Damage;
        this.radius = Radius;
        this.moveSpeed = MoveSpeed;
        rotateSpeed = RotateSpeed;
        isgoaway = false;
    }

    private void Update()
    { 
        if (isgoaway == true)
        {
            transform.position = Vector3.Lerp(transform.position, preDir * 15, 1 * Time.deltaTime);

        }

        
        if (TargetPos == null)
        {
            if (isgoaway == false)
            {
                isgoaway = true;
                MonoController.Instance.Invoke(2,()=>ObjectPool.Instance.PushObject(this.gameObject));

            }
            
            return;
        }
       

        SelfCircle(); 
        preDir = transform.position -TargetPos.position;
        float dis = (transform.position - TargetPos.position).magnitude;
        if (dis >= radius)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPos.position , moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(TargetPos.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, TargetPos.position.y +  0.5f, transform.position.z),1*Time.deltaTime);
    }
    private void SelfCircle()
    {
        transform.Rotate(30 * Time.deltaTime, 60 * Time.deltaTime, 60 * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Mob m = other.GetComponent<Mob>();
            if (m.Health - damage <= 0)
            {
                CardSlot.Instance.AddSunCont(15);
            }
            SoundSystem.Instance.PlayRandom2Dsound("Ignite");
            m.Hurt(damage, 0, DamageType.normal);
        }
    }
}
