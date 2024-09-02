using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DamageNum : MonoBehaviour
{

    private GameObject singleNum;
    public Sprite[] numSprite;
    private float destroyTime = 0.6f;
    private float UpSpeed;
    public void Init(int num)
    {
        Invoke("Destroy", destroyTime);
        singleNum = FileLoadSystem.ResourcesLoad<GameObject>("UI/Terraria/SingleNum");
        string numstring = num.ToString();
        for (int i = 0; i < numstring.Length; i++)
        {
            GameObject g = Instantiate(singleNum, transform);
            
            g.GetComponent<Image>().sprite = numSprite[Convert.ToInt32(numstring[i] - 48)];
        }
        UpSpeed = 4;

    }
    private void Update()
    {
        if (UpSpeed > 0)
        {
             UpSpeed -= 10 * Time.deltaTime;
            transform.position += new Vector3(0, UpSpeed, 0);
        }
        
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
