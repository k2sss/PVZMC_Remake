using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandDirtAttack_Boss : MonoBehaviour
{
    private Vector3 Dir;
    private float speed;
    private float acceleration;
    public void Init(Vector3 Direction,float BulletSpeed,float BulletAcceleration)
    {
        transform.right = -Direction;
        this.Dir = Direction;
        this.speed = BulletSpeed;
        this.acceleration = BulletAcceleration;
    }
    private void Update()
    {
        speed += acceleration * Time.deltaTime;
        transform.Translate((speed * Dir.normalized)*Time.deltaTime, Space.World);
    }
}
