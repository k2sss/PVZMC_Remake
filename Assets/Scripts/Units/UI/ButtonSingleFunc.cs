using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSingleFunc : MonoBehaviour
{
    public MyEvent buttonEvent;
    public string ClickSound = "Click";
    public Vector4 HighLightColor;
    public bool IsSelectRenderer;
    public Renderer SelectedRenderer;
    private Renderer _renderer;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }
    private void OnMouseEnter()
    {
        ChanngeColor(HighLightColor);
    }

    private void OnMouseExit()
    {
        ChanngeColor(new Vector4(1, 1, 1, 1));
    }
    private void OnMouseDown()
    {
        
        if (UIMgr.Instance.UIStackCount == 0 )
        {
            
            if (ClickSound != "")
            {
                SoundSystem.Instance.Play2Dsound(ClickSound);
            }
            buttonEvent?.Invoke();
        }
    }
    private void ChanngeColor(Vector4 color)
    {   if(IsSelectRenderer == false)
        for (int i = 0; i < _renderer.materials.Length; i++)
        {
            
            _renderer.materials[i].color = color;
            
        }
        else
            {
            for (int i = 0; i < SelectedRenderer.materials.Length; i++)
            {

                SelectedRenderer.materials[i].color = color;

            }
        }
    }

}
