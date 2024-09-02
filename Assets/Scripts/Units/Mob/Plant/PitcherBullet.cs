using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherBullet : Bullet
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 betweenPos;
    private float t;
    public Vector3 RotVector3;
    public float Speed = 1;

    public void InitPictherBullet(Vector3 targetPos,float Damage,float height = 10)
    {
        t = 0;
        startPos = transform.position;
        endPos = targetPos;
        betweenPos = startPos + (endPos - startPos)/2 + new Vector3(0,height,0);

        this.Damage = Damage;
    }
    protected virtual void Update()
    {
        transform.Rotate(RotVector3 * Time.deltaTime);
        t += Time.deltaTime * Speed;
        Vector3 a = (1 - t) * (1 - t) * startPos;
        Vector3 b = 2 * t * (1 - t) * betweenPos;
        Vector3 c = t * t * endPos;
        Vector3 d = a + b + c;
        transform.position = d;

    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        OnHit(other);
    }

}
