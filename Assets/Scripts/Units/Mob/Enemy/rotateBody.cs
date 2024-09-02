using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateBody : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;     //射线
        int Rmask = LayerMask.GetMask("ik");
        Vector3 Point_dir = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, Point_dir, out hit, 50.0f, Rmask))
        {
            Quaternion NextRot = Quaternion.LookRotation(Vector3.Cross(hit.normal, Vector3.Cross(transform.forward, hit.normal)), hit.normal);
            GetComponent<Rigidbody>().MoveRotation(Quaternion.Lerp(transform.rotation, NextRot, 0.1f)); //旋转
        }

    }
}
