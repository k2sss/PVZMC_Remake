using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public int value = 25;
    public float DisapearTime = 40;
    private float DisapearTimer;
    public float MoveSpeed;
    public float FallSpeed;
    public bool _selected;
    public Rigidbody rb;
    public AudioClip pickclip;
    private void Start()
    {
      
        init();
        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.SunMore == true)
        {
            value = 50;
        }
    }

    void Update()
    {
        SelfCircle();
        GoToSunManager();
        TimeDisapear();
    }
    public void init()//初始化
    {
        _selected = false;
        DisapearTimer = 0;
        UnLockRbPos();
        gameObject.tag = "Sun";
    }
    private void TimeDisapear()
    {
        DisapearTimer += Time.deltaTime;
        if (DisapearTimer > DisapearTime)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }

    private void SelfCircle()
    {
        transform.Rotate(30 * Time.deltaTime, 60 * Time.deltaTime, 60 * Time.deltaTime);
    }
    private void GoToSunManager()//移动到指定地点
    {
        if (_selected == true)
            transform.position = Vector3.Lerp(transform.position, SunManager.Instance.transform.position, MoveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_selected == false)
        {
            if (other.CompareTag("Player"))
            {
                Pick();
            }

        }
    }
    public void Pick()
    {
        LockRbPos();
        gameObject.tag = "Untagged";
        SoundSystem.Instance.Play2Dsound(pickclip);
        if (CardSlot.Instance != null)
        {
            CardSlot.Instance.AddSunCont(value);
            CardSlot.Instance.AllCardsSetConsumeMask();
        }

        _selected = true;
        Invoke("SetF", 2);
    }
    private void SetF()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }

    private void LockRbPos()//锁定刚体Pos
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }
    private void UnLockRbPos()//解锁刚体Pos
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;

    }
}
