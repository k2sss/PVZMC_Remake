using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HandlerPannel : MonoBehaviour,IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Vector2 Downpos;//记录原点位置
    public GameObject Handler_Out;
    public GameObject Handler_In;
    private RectTransform Handler_Out_RectTransform;
    private RectTransform Handler_In_RectTransform;
    public Vector2 OutPut;
    public float Radium;
    private void Start()
    {
        Handler_Out_RectTransform = Handler_Out.GetComponent<RectTransform>();
        Handler_In_RectTransform = Handler_In.GetComponent<RectTransform>();
        Handler_Out.SetActive(false);
    }
    public void OnDrag(PointerEventData eventData)
    {

        Vector2 dir = Vector3.ClampMagnitude(eventData.position - Downpos, Radium) ;
        Handler_In_RectTransform.position = Downpos + dir;
        OutPut = dir.normalized;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Downpos = eventData.position;
       
        Handler_Out.SetActive(true);
        Handler_Out_RectTransform.position = Downpos; 
        Handler_In_RectTransform.position = Downpos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Handler_Out.SetActive(false);
        OutPut = new Vector2(0, 0);
    }


}
