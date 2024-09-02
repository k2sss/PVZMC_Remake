using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Dynamic;
[RequireComponent(typeof(BoxCollider))]
public class AttackBox : MonoBehaviour
{
    public AttackBoxType type;
    public float Damage;
    public float PlayerDamage;
    public float DamagePeneTration;
    public DamageType damageType;
    public bool EnterPoolWhenHitTarget;
    public bool IsMove;
    public float MoveSpeed;
    private Vector3 direction;
    private Rigidbody rb;
    public BuffType bufftype;
    public float bufftime;
    public Action<Collider> triggerEnterEvent { get; set; }
    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
    public void InitMove(float speed,Vector3 direction,Vector3 transfromRight)
    {
        IsMove = true;
        this.MoveSpeed = speed;
        this.direction = direction;
        this.transform.right = transfromRight; 
    }
    private void Start()
    {
        if (IsMove == true)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    private void FixedUpdate()
    {
        if (IsMove == true)
        {
            rb.velocity = direction.normalized * MoveSpeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        triggerEnterEvent?.Invoke(other);
        TriggerEnter(other);
        
    }
    private void AddBuff(Mob target)
    {
        target.AddBuff(bufftype, bufftime);
    }
    private void EnterPool()
    {
            if (EnterPoolWhenHitTarget == true)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }

    public void TriggerEnter(Collider other)
    {
        if (type == AttackBoxType.enemy)
        {
            if (other.gameObject.layer == 9)
            {
                if (other.CompareTag("Player"))
                {
                    Mob e = other.GetComponent<Mob>();
                    e.Hurt(PlayerDamage, DamagePeneTration, damageType);
                    AddBuff(e);
                    EnterPool();
                }
                else
                {
                    Mob e = other.GetComponent<Mob>();
                    e.Hurt(Damage, DamagePeneTration, damageType);
                    AddBuff(e);
                    EnterPool();
                }
                    


            }
        }
        else if (type == AttackBoxType.plants)
        {
            if (other.CompareTag("Enemy"))
            {
                Mob e = other.GetComponent<Mob>();
                e.Hurt(Damage, DamagePeneTration, damageType);
                AddBuff(e);
                EnterPool();
            }
        }
        if (other.CompareTag("Blocks"))
        {
            other.GetComponent<FunctionalBlock>().CauseDamage(Damage, BlockStrengthType.normal, 0);
            EnterPool();
        }
    }
}

public enum AttackBoxType
{
    enemy,
    plants,
}
