using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSetActive : MonoBehaviour
{
    private float Timer;
    public float time = 5;
    public bool EnterPool = true;
    public void SetTimer(float time)
    {
        Timer = time;   
    }
    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > time)
        {
            if (EnterPool == false)
            {
            gameObject.SetActive(false);
            Timer = 0;  
            }
            else
            {
                ObjectPool.Instance.PushObject(gameObject);
                gameObject.SetActive(false);
                Timer = 0;
            }
           
        }
    }
    private void OnEnable()
    {
        if(EnterPool == true)
        Timer = 0;
    }



}
