using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaster_blaster_damage : MonoBehaviour
{
    public float DamagePer0point1;
    public List<Enemy> targets = new List<Enemy>();
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            timer = 0;
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].Hurt(DamagePer0point1);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targets.Add(other.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targets.Remove(other.GetComponent<Enemy>());
        }
    }
}
