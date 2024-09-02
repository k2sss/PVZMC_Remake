using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusLeaf : Plants
{
    public Grid grid;
    protected override void Awake()
    {
        base.Awake();
        grid.gameObject.SetActive(false);
        
    }
    protected override void Start()
    {
        base.Start();
        MonoController.Instance.Invoke(0.1f, () => grid.gameObject.SetActive(true));
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        if (grid.corePlants != null && grid.corePlants.Health > 0)
            grid.corePlants.Hurt(Damage, penetration, Dtype);
        else
            base.Hurt(Damage, penetration, Dtype);
    }

}
