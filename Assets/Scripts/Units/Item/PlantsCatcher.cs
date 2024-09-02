using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlantsCatcher : MonoBehaviour
{
    private GameObject catchedPlant0bject;
    private Plants catchedPlantScript;
    public bool isCatched { get; private set; }
    


    private void Update()
    {
        if (catchedPlant0bject == null)
            return;
        catchedPlant0bject.transform.position = transform.position + Vector3.up + Vector3.right *1/2f;
    }
    public void CatchPlant(Plants targetPlant)
    {
        if (targetPlant == null)
            return;
        if (!targetPlant.IsCatchable())
            return;
        isCatched = true;
        catchedPlantScript = targetPlant;
        catchedPlant0bject = targetPlant.gameObject;

        if (catchedPlantScript.targetGrid != null) 
            catchedPlantScript.targetGrid.RemovePlantBind();

    }
    public void PutPlant(Grid targetGrid)
    { 
        isCatched = false;
        if (catchedPlant0bject == null)
            return;

       
        catchedPlant0bject.transform.position = targetGrid.transform.position;
        targetGrid.BindPlant(catchedPlantScript);
        catchedPlant0bject = null;
        return;
    }
    public PlantsType GetCatchePlantType()
    {
        return catchedPlantScript.type;
    }


}
