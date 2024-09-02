using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRigidbody : MonoBehaviour
{
    public float Height = 0.7f;
    public float Gravity = 10;
    public float Speed;
    
    private void Update()
    {
        if (RayCheck() == false)
        {
            Fall();
            Speed += Gravity * Time.deltaTime;
        }
        else
        {
            Speed = 0;
        }
        SelfCircle();
    }

    private bool RayCheck()
    {
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Height, 1 << 3))
        {
            return true;
        }
        return false;


    }
    private void Fall()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y -Speed*Time.deltaTime, transform.position.z);
    }
    private void SelfCircle()
    {
       
        transform.eulerAngles += new Vector3(0, 1, 0);
    }



}
