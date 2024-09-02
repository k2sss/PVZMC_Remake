using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhenGetPlantPannel : Pannel
{
   
    public Text plantsName;
    public Text Card_PlantConsume;
    public Image Card_PlantCardImage;
    public static WhenGetPlantPannel Instance;
    private GameObject target;
    private void Awake()
    {
        target = transform.GetChild(0).gameObject;
        target.SetActive(false);
        Instance = this;
    }
    public void Show(PlantsType type)
    {
        target.SetActive(true);
        GetComponent<Animator>().SetBool("Bool", true);
        Plant_Scriptable plantInfo = ResourceSystem.Instance.GetPlants(type);
        plantsName.text = plantInfo.myName;
        Card_PlantCardImage.sprite = plantInfo.sprite;
        Card_PlantConsume.text = plantInfo.Consume.ToString();
    }
    public void SetActiveFalse()
    {
        GetComponent<Animator>().SetBool("Bool", false);
        target.SetActive(false);
    }
}
