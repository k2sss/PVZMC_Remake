using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderManCheckBox : MonoBehaviour
{
    public EnderMan enderMan;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            enderMan.ToOtherLine();
            enderMan.BecomeAngry();
        }
    }
}
