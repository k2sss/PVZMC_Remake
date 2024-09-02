using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ChargingShooter : Plants
{
    private Mob target;
    private Mob templeTarget;
    public Transform shootpos;
    public MultiAimConstraint aimConstraint;
    public Transform targetAimPos;
    public GameObject chargingLight;

    protected override void Update()
    {
        base.Update();
        HurtRig();
        Debug.DrawRay(shootpos.position, shootpos.up * 10, Color.red);
        target = MyPhysics.BoxRayCheck<Mob>(shootpos.position + new Vector3(1, 0, 0), 2, 20f, Vector3.right, 20, 8);
        if (target != null)
        {
            targetAimPos.position = Vector3.Lerp(targetAimPos.position, target.transform.position + Vector3.up / 2, 5 * Time.deltaTime);
            aimConstraint.weight = Mathf.Lerp(aimConstraint.weight, 1, 2 * Time.deltaTime);
            _animator.SetBool("IsAttack", true);
            IsAttacking = true;
        }
        else
        {
            aimConstraint.weight = Mathf.Lerp(aimConstraint.weight, 0, 1 * Time.deltaTime);
            _animator.SetBool("IsAttack", false);
            IsAttacking = false;
        }
    }
    public void CauseDamage(float damage)
    {
        if (templeTarget != null)
        {
            templeTarget.Hurt(damage * FinalDamage);
        }
    }

    public void Fire()
    {
        if (target != null)
        {
        
            GameObject bullet = Instantiate(chargingLight); 
          
            ChargingLight c = bullet.GetComponent<ChargingLight>();

            templeTarget = target;

            c.SetShoot(shootpos, target as Enemy);
            
        }

    }
}
