using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutWall : Plants
{
    public Material normal;
    public Material hurt;
    public Material BadHurt;
    public ParticleSystem particle;
    protected override void Start()
    {
        base.Start();
        particle = transform.Find("NutsParticle").GetComponent<ParticleSystem>();
    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {   Check();
        base.Hurt(Damage);
        particle.Emit(10);
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();
    }
    private void Check()
    {
        if(WithInRange(Health,MaxHealth*(2f/3),MaxHealth))
        {
          
            ChangeMaterial(normal);
            
        }
        else if (WithInRange(Health, MaxHealth * (1f / 3), MaxHealth * (2f / 3)))
        {
    
            ChangeMaterial(hurt);
        }
        else if (WithInRange(Health, 0, MaxHealth * (1f / 3)))
        {

            ChangeMaterial(BadHurt);
        }

    }
    private bool WithInRange(float x,float min, float max)
    {
        if (min<=x && x <= max)
        {
            return true;
        }
        return false;
    }
    public override void ChangeMaterial(Material mat)
    {
        particle.Emit(10);
        base.ChangeMaterial(mat);
    }

}
