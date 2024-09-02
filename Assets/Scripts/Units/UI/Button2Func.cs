using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTwoFunc : MonoBehaviour
{
    public MyEvent aevent;
    public MyEvent bevent;
    public string ClickSound;
    public Vector4 HighLightColor;
    private Renderer _renderer;
    private bool a;
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
        if (UIMgr.Instance.UIs.Count == 0)
        {

            if (ClickSound != "")
            {
                SoundSystem.Instance.Play2Dsound(ClickSound);
            }
            if (a == false)
            {
                aevent?.Invoke();
                a = true;
            }
            else
            {
                bevent?.Invoke();
                a = false;
            }
        }

    }
    private void ChanngeColor(Vector4 color)
    {
        for (int i = 0; i < _renderer.materials.Length; i++)
        {
            _renderer.materials[i].color = color;
        }
    }

}
[System.Serializable]
public class MyEvent : UnityEvent { }