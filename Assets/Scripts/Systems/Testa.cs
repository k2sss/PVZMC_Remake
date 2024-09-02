using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testa : MonoBehaviour
{
    public Button button;
    private void Start()
    {
        button.onClick.AddListener(Test);
    }
    public void Test()
    {
        Debug.Log("Test");
    }
}
