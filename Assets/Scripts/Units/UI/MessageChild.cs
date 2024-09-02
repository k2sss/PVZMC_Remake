using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageChild : MonoBehaviour
{
    private Text textElement;
    private Color targetColor;
   
    public void Init(string text, Color color)
    {
        textElement = gameObject.GetComponent<Text>();
        Set(text, color);
        Invoke("Destroy", 8);
    }
    private void Update()
    {    targetColor.a = (((Mathf.Sin(Time.time * 4) + 1f) / 4f) + 0.5f);
        textElement.color = targetColor;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void Set(string text,Color color)
    {
        textElement.text = text;
        targetColor = color;
    }


}
