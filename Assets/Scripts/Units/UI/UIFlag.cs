using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFlag : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Trigger()
    {
        _animator.SetBool("Open", true);
    }
}
