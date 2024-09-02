using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPitcherBullet : PitcherBullet
{

    protected override void OnTriggerEnter(Collider other)
    {
        OnHit(other);
        other.GetComponent<Mob>().AddBuff(BuffType.Vulnerable,5);
    }
}
