using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlock : MonoBehaviour
{
    public float jumpforce;
    public float forwardForce;
    private float CircleTimer;
    public float CircleTime;
    private Collider _collider;
    private void Start()
    {
        _collider = GetComponent<Collider>();
    }
    private void Update()
    {   CircleTimer += Time.deltaTime;
        if (CircleTimer > CircleTime && CircleTimer < CircleTime + 0.1f)
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;

            if (CircleTimer > CircleTime + 0.1f)
            {
                CircleTimer = 0;
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = new Vector3(rb.velocity.x - forwardForce, rb.velocity.y + jumpforce, rb.velocity.z);
        }
    }
}
