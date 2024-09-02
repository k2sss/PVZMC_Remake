using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBlock : FunctionalBlock
{
    public float AttenuationRate = 1;
    private void OnCollisionEnter(Collision collision)
    {
        SoundSystem.Instance.PlayRandom2Dsound("Slime");
        Collider other = collision.collider;
        if (other.CompareTag("Player"))
        {
            Debug.Log("1");
            PlayerMoveController controller = other.GetComponent<PlayerMoveController>();
            Rigidbody rb = controller._rigidbody;
            rb.velocity = new Vector3(rb.velocity.x, AttenuationRate * -controller.Yspeed, rb.velocity.z);
        }
        else if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            Rigidbody rb = enemy.rb;
            rb.velocity = new Vector3(rb.velocity.x, AttenuationRate * -enemy.Yspeed, rb.velocity.z);
        }
    }
    
}
