using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class target : MonoBehaviour
{
    public Transform body;                  //身体
    public LayerMask terrainLayer;          //检测图层
    Vector3 prePos; //位置
    public TwoBoneIKConstraint ikC;
    public Transform offset;
    public float stepstance = 0.8f;                //步长
    private float high = 0.2f;               //高度
    private float speed = 3;                 //速度
    float lerp = 1;


    public target leg1, leg2;                //约束

    private void Awake()
    {
        prePos = transform.position;
    }

    void Update()
    {

        Ray ray = new Ray(offset.position, -body.up);
        //Debug.DrawRay(offset.position, -body.up *3);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3, 1 << 3))
        {
            if ((hit.point - prePos).magnitude > stepstance && leg1.lerp >= 1 && leg2.lerp >= 1)
            {
                lerp = 0;
                prePos = hit.point;
                // targets[i].position = hit.point;

            }
            ikC.weight = 1;
        }
        else
        {
            ikC.weight = 0.3f;
        }



        if (lerp < 1)
        {
            lerp += Time.deltaTime * speed;
            transform.position = Vector3.Slerp(transform.position, prePos, lerp);
            float y = transform.position.y;
            y += Mathf.Sin(lerp * Mathf.PI) * 0.2f;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

        }
        else
        {
            transform.position = prePos;
        }
    }

}
