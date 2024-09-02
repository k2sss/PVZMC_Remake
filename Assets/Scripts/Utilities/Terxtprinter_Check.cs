using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextPrinter))]
public class Terxtprinter_Check : MonoBehaviour
{
    private TextPrinter printer;
    private Transform PlayerTransform;
    public float Range = 1;
    private bool IsPrint;
    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        printer = GetComponent<TextPrinter>();
    }
    private void Update()
    {
        if ((PlayerTransform.position - transform.position).magnitude <= Range&&IsPrint == false)
        {
            IsPrint = true;
            printer.StartPrintNextSentence();
        }
       
    }
}
