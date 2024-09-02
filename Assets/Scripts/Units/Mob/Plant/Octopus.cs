using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : Plants
{
    private bool isGetTarget;
    public AttackBox attackBox;
    public Collider attackCollider;

    protected override void Update()
    {
        base.Update();
        HurtRig();
        attackBox.Damage = FinalDamage;
    }
    protected override void HurtRig()
    {
        base.HurtRig();
        InvokeRepeating("Check", 0, 0.1f);
    }
    public void EnableAttackBox(int T)
    {
        if(T == 0)
        attackCollider.enabled = false;
        else
            attackCollider.enabled = true;
    }
    public void Check()
    {
        isGetTarget = false;
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in Enemys)
        {
            Vector3 offset = e.transform.position - transform.position;
            if( Mathf.Abs(offset.x) <3&& Mathf.Abs(offset.y) < 2f && Mathf.Abs(offset.z) < 3 )
            {
                isGetTarget = true;
            }
        }
        if (isGetTarget == true)
        {
            _animator.SetBool("IsAttack", true);
            IsAttacking = true;

        }
        else
        {
            _animator.SetBool("IsAttack", false);
            IsAttacking = false;
        }
    }
}
