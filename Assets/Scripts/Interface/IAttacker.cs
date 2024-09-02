using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker 
{
    public abstract void AttackOther(float Damage);
    public abstract float AttackMultiplier();

    public abstract string GetAttackerName();
   
}
