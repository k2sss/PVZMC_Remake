using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingLight : MonoBehaviour
{
    private Transform StartTransform;
    private Enemy target;
    private bool isStart;
    public GameObject Particle1;
    private void Update()
    {
        if (isStart == true)
        {
        Vector3 endPos = target.transform.position + Vector3.up * target.aimHeight;
        float length = (endPos - StartTransform.position).magnitude;
        transform.localScale = new Vector3(length, transform.localScale.y, transform.localScale.z);
        transform.position = StartTransform.position;
        transform.right = endPos - StartTransform.position;
        }
    }
    public void Explode()
    {
        Vector3 endPos = target.transform.position + Vector3.up * target.aimHeight;
        GameObject a =  Instantiate(Particle1,transform);
        a.transform.position = StartTransform.position;
        a.transform.right = endPos - StartTransform.position;

        float length = (endPos - StartTransform.position).magnitude;
        ParticleSystem s = a.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule shape = s.shape;
        shape.scale = new Vector3(length, s.shape.scale.y, s.shape.scale.z);
        shape.position = new Vector3(length / 2, 0, 0);

        
    }
    public void SetShoot(Transform startPos,Enemy target)
    {
        this.StartTransform = startPos;
        this.target = target;
        isStart = true;
    }
}
