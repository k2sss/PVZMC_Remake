using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : BaseManager<WeatherManager>
{

    private Color defaultColor = Color.white;
    private float defaultIntensity = 1;
    private Light dirlight;
    public bool IsBloodMoon { get; private set; }

    private void InitLight()
    {
        if(dirlight == null)
        {
            Light directLight;
            directLight = GameObject.Find("Directional Light").GetComponent<Light>();
            if (directLight == null)
            {
                Debug.Log("无法在当前场景中找到平行光");
                return;
            }
            dirlight = directLight;
        }
    }

    public void TurnToBloodMoon()
    {

        //设置 平行灯光
        if (!MySystem.IsInLevel())
            return;
        InitLight();
        Light directLight = dirlight;
        defaultColor = directLight.color;
        defaultIntensity = directLight.intensity;
        directLight.intensity = 0.4f;
        directLight.color = Color.red;
        IsBloodMoon = true;
    }
    public void TurnToDefault()
    {
        if (!MySystem.IsInLevel())
            return;
        InitLight();
        Light directLight = dirlight;
        directLight.color = defaultColor;
        directLight.intensity = defaultIntensity;
        IsBloodMoon = false;
    }


}
