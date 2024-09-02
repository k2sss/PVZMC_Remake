using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBox : MonoBehaviour
{
    public BuffType buffType;
    public float buffTime;
    public bool isperisitent;
    public AttackBoxType atype;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")&&atype == AttackBoxType.plants)
        {

        }
        else if(other.CompareTag("Plants") && atype == AttackBoxType.enemy)
            {

            }
    }
}
