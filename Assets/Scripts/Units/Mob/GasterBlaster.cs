using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasterBlaster : Plants
{
    bool IsMove;
    private float speed = 0;
    public AudioClip[] clips;
    public Gaster_blaster_damage GBdamage;
    protected override void Start()
    {
        base.Start();
        if (targetGrid != null)
        {
            targetGrid.RemovePlantBind();//灰烬类植物需要解开与GRID的绑定
        }
           
        _animator.SetBool("Attack", true);
        MonoController.Instance.Invoke(1,()=> _animator.SetBool("Attack2", true));
        PlayOneShot(clips[0]);
        GBdamage.DamagePer0point1 = FinalDamage;
        gameObject.tag = "Untagged";
    }
    protected override void Update()
    {
        base.Update();
        if (IsMove == true)
        {
            speed += 20 *Time.deltaTime;
            transform.Translate(-speed*Time.deltaTime, 0, 0,Space.World);
        }
    }
    void StartMove()
    {
        IsMove = true;
        PlayOneShot(clips[1]);
    }
    void Shake()
    {
        CameraAction.Instance.StartShake();
    }
}
