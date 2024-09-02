using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Enemy,Ipurefiable
{
    // Start is called before the first frame update
    public Transform rayOrigin;
    public float rayLength;
    public GameObject box;
    private bool isReadyForNextAttack = true;
    protected override void Start()
    {
        base.Start();
        Walk();
        GenerateHpBar();
    }
    protected override void Update()
    {
        base.Update();
        target = MyPhysics.BoxRayCheck<Mob>(rayOrigin.position, 1, Vector3.left, rayLength, 9);
        if (!isReadyForNextAttack) return;
        if (target != null)
        {
            StartAttack();
        }
        else
        {
            EndAttack();
        }
    }
    private void FixedUpdate()
    {

        if (CantMove == false && IsVertigo == false)
        {
            rb.velocity = new Vector3(-FinalSpeed, rb.velocity.y, 0);
            MoveToTheNearestRow();
        }
    }
    public override void StartAttack()
    {

        if (IsAttacking == false)
        {
            _animator.SetBool("IsAttack", true);
            CantMove = true;
            IsAttacking = true;
            isReadyForNextAttack = false;
        }
    }
    public void SetIsReadyForNextAttack()
    {
        isReadyForNextAttack = true;
    }
    public void Attack()
    {
        box.SetActive(true);
        MonoController.Instance.Invoke(0.2f,()=>box.SetActive(false));
        SoundSystem.Instance.Play2Dsound("Shock");

    }
    public void PlayFallDownSound()
    {
        SoundSystem.Instance.Play2Dsound("Shock");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Blocks"))
        {
            collision.gameObject.GetComponent<FunctionalBlock>().CauseDamage(1000,BlockStrengthType.normal,0);
        }
    }
   
    public override void Hurt(float Damage, float penetration = 0, DamageType Dtype = DamageType.normal)
    {
        ShowDamage(Damage);
        base.Hurt(Damage, penetration, Dtype);
    }

    void Ipurefiable.OnPurify()
    {
        SetNowSpeed(-0.3f);
        SetAttackSpeed(-0.5f);
        isPurified = true;
    }

    void Ipurefiable.OnEndPurify()
    {
        SetNowSpeed(0.3f);
        SetAttackSpeed(0.5f);
        isPurified = false;
    }
}
