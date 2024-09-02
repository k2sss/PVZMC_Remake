using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroy : MonoBehaviour
{
    private float Timer;
    public float time = 5;
    public void SetTimer(float time)
    {
        Timer = time;
    }
    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > time)
        {
            Destroy(gameObject);
            Timer = 0;
        }
    }


}
