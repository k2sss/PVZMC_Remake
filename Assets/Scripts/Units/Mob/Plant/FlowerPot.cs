using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPot : Plants
{
    public Grid grid;


    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        if(grid.corePlants != null && grid.corePlants.Health > 0)
            grid.corePlants.Hurt(Damage, penetration, Dtype);
        else
            base.Hurt(Damage, penetration, Dtype);
    }


}