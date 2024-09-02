using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Enemy,Ipurefiable
{
    public GameObject thorn;
    protected override void Start()
    {
        base.Start();
        GenerateHpBar();
        Walk();
    }
    protected override void Update()
    {
        base.Update();
        HurtRig();

    }
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        base.Hurt(Damage, penetration, Dtype);
        ShowDamage(Damage);
    }
    private void FixedUpdate()
    {
        if (CantMove == false && IsVertigo == false)
        {
            MoveToTheNearestRow();
        }
    }
    public void JumpForward()
    {
        if (CantMove == false && IsVertigo == false)
        {
            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
        }
    }
    public override void Death()
    {
        base.Death();
        GenerateThron();

    }
    public void GenerateThron()
    {
        GameObject g = Instantiate(thorn);
        Thorn t1 = g.GetComponent<Thorn>();
        t1.info_int_1 = 3;
        float x = ((int)transform.position.x + 0.5f);
        float y = ((int)transform.position.y + 1);
        float z = ((int)transform.position.z + 0.5f);
        g.transform.position = new Vector3(x, y, z);

        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.2f, 1 << 6))
        {
            Grid gr = hit.collider.GetComponent<Grid>();
                if (gr.IsInfectable())
                {
                    gr.Transfer2();
                }
        }
        WorldManager.UpdateWorldInfo();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Blocks"))
        {
            collision.gameObject.GetComponent<FunctionalBlock>().CauseDamage(1000, BlockStrengthType.normal, 0);
        }
    }

    void Ipurefiable.OnPurify()
    {
        SetNowSpeed(-0.5f);
        isPurified = true;
    }

    void Ipurefiable.OnEndPurify()
    {
        SetNowSpeed(0.5f);
        isPurified = false;
    }
}
