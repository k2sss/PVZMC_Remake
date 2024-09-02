using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRotate : MonoBehaviour
{
    public Material sky;
    public float rot;
    public float RotateSpeed;
    void Update()
    {
        if (rot > 360)
            rot = 0;

        rot += RotateSpeed *Time.deltaTime;
        sky.SetFloat("_Rotation", rot);
    }
}
