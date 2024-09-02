using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotChangeRotation : MonoBehaviour
{
    public Vector3 Startrotation;
    private void Start()
    {
        Startrotation = transform.eulerAngles;
    }
    private void Update()
    {
        transform.eulerAngles = Startrotation;
    }
}
