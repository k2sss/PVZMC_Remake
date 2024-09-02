using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationMessage : BaseManager<InformationMessage>
{
    [SerializeField]private GameObject textChild;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    public void Init()
    {

    }
    public void WriteMessage(string text,Color textColor)
    {
       GameObject textChildObject = Instantiate(textChild,transform);
        textChildObject.GetComponent<MessageChild>().Init(text,textColor);
       
    }

}
