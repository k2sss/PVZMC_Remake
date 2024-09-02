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
                Debug.Log("�޷��ڵ�ǰ�������ҵ�ƽ�й�");
                return;
            }
            dirlight = directLight;
        }
    }

    public void TurnToBloodMoon()
    {

        //���� ƽ�еƹ�
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
