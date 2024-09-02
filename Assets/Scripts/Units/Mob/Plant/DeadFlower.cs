using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadFlower : Plants
{
    public GameObject sun2;
    public AudioClip xishou;
    protected override void Start()
    {
        base.Start();
        Invoke("Dead", 10);
    }
    [ContextMenu("change")]
    public void Turn()
    {
        _audioSource.PlayOneShot(xishou);
        GameObject[] a = GameObject.FindGameObjectsWithTag("Sun");
        for (int i = 0; i < a.Length; i++)
        {
            GameObject go = ObjectPool.Instance.GetObject(this.sun2);
            go.transform.position = a[i].transform.position;
            Sun2 sun2_scr = go.GetComponent<Sun2>();
            sun2_scr.Init(6, 2, Random.Range(1f,2f), 160 + Random.Range(0,20), transform);
            ObjectPool.Instance.PushObject(a[i]);
        }
    }
    protected override void Update()
    {
        HurtRig();
        base.Update();
    }
    private void Dead()
    {
        _animator.SetBool("disappear", true);
    }


}
