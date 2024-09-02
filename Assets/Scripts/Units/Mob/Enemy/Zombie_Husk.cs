using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Husk : Zombie
{
    public override void Attack()
    {
        // base.Attack();
        if (type != EnemyType.zombie_Diamond)
        {
            if (target != null)
            {
                target.Hurt(FinalDamage);
                if (target.CompareTag("Player"))
                    target.gameObject.GetComponent<PlayerBuff>().AddBuff(BuffType.hungry, 3);

                PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds);
            }
            else if (targetBlock != null)
            {

                targetBlock.CauseDamage(FinalDamage, BlockStrengthType.normal, 0, 1, 0.3f);
                PlayRandomSounds(ResourceSystem.Instance.GetEnemy(type).AttackSounds, 0.3f);
            }
        }
        else
        {
            base.Attack();
        }
    }

}
