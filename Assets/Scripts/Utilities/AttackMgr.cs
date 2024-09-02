using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMgr
{
    
    public static void Attack(IAttacker Attacker,Mob target,float Damage)
    {
        Attacker.AttackOther(Damage);
        target.Hurt(Damage * Attacker.AttackMultiplier(),Attacker);
    }
    public static void AttackWithBuff(IAttacker Attacker, Mob target, float Damage,BuffType bufftype)
    {
        Attack(Attacker, target, Damage);
        if (!target.IsDead)
            target.AddBuff(bufftype);
    }
    public static void AttackWithBuff(IAttacker Attacker, Mob target, float Damage, BuffType bufftype,float buffTime)
    {
        Attack(Attacker, target, Damage);
        if(!target.IsDead)
            target.AddBuff(bufftype, buffTime);
    }
}

