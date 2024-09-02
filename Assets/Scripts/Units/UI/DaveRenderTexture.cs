using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaveRenderTexture : MonoBehaviour
{
    public string CameraPath;
    private RawImage rawImage;
    private Camera cam;
    private RenderTexture m_renderTexture;
    // Start is called before the first frame update
    void Start()
    {
        MonoController.Instance.Invoke(0.1f, () =>
        {
            cam = GameObject.Find(CameraPath).GetComponent<Camera>();
            rawImage = GetComponent<RawImage>();
            m_renderTexture = new RenderTexture(1080, 1080, 32, UnityEngine.Experimental.Rendering.DefaultFormat.LDR);
            rawImage.texture = m_renderTexture;
            cam.targetTexture = m_renderTexture;
            cam.clearFlags = CameraClearFlags.SolidColor;
           //CameraClearFlags.SolidColor时会有背景色，需要设置背景色透明
            Color color = cam.backgroundColor;
            color.a = 0f;
            cam.backgroundColor = color;

        });
      
      
    }

    // Update is called once per frame
   
}
