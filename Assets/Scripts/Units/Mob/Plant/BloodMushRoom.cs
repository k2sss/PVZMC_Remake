using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMushRoom : Plants
{
    public GameObject gravityCircle;

    protected override void Start()
    {
        base.Start();
        gravityCircle.SetActive(true);
        MonoController.Instance.Invoke(10, Death);
        
    }
}
