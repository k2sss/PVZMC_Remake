using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Event2 : MonoBehaviour
{
    TextPrinter printer;
    public TextPrinter printer2;
    public Light global_light;
    public GameObject[] airWalls;
    private float timer;
    private bool StartTimeCount;
    private void Start()
    {
        Color R_color = global_light.color;
        printer =GetComponent<TextPrinter>();
    }
    private void Update()
    {
        if (StartTimeCount == true)
        {
            timer += Time.deltaTime;
            if (timer > 30)
            {
                printer2.StartPrintNextSentence();
                printer2.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position +new Vector3(0,1,0);
                timer = 0;
                StartTimeCount = false;
            }
        }

    }
    public void CancelAirWall()
    {
        for (int i = 0; i < airWalls.Length; i++)
        {
            airWalls[i].gameObject.SetActive(false);
        }
    }
}
