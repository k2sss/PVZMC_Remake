using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : BaseManager<PlantManager>//负责种植植物的操作
{
    public GameObject SelectedPlant { get; private set; }
    private Card TargetCard;
    public GameObject PlantParticles;
    public bool AllPlantsHPtoOne;
    public float DamageAdder;
    public float AttackSpeedAdder;

    public bool isRandomPlantType { get; set; }
    private int isPlantTypeProbability;
    public int IsPlantTypeProbability
    {
        get
        {
            return isPlantTypeProbability;
        }
        set
        {
            isPlantTypeProbability = Mathf.Clamp(value, 0, 100);
        }
    }
public float clickTimer { set; get; }
    private void Start()
    {

    }

    private void Update()
    {
        clickTimer -= Time.unscaledDeltaTime;
        if (Input.GetMouseButtonDown(0) && clickTimer < 0f)
        {
            if (!isRandomPlantType)
                SetPlant(SelectedPlant);
            else
            {
                if (Random.Range(0, 100) < isPlantTypeProbability)
                    SetRandomPlant();
                else
                    SetRandomEnemy();
                    
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            EmptySelectedPlant();
        }
    }
    public void GetSelectedPlant(PlantsType type, Card Target)
    {
        clickTimer = 0.1f;
        TargetCard = Target;
        SelectedPlant = ResourceSystem.Instance.GetPlants(type).prefab;
        Time.timeScale = 0.2f;
        CameraAction.Instance.ChangeRotation(new Vector3(65, 13, 14));
        PhoneControlMgr.Instance.SetActiveToF(false);
    }//从CARD那里获取PREFAB
    public void EmptySelectedPlant()
    {
        if (TargetCard != null)
            TargetCard.CancleSelect();
        SelectedPlant = null;
        if (Time.timeScale > 0.1f)
            Time.timeScale = 1f;
        CameraAction.Instance.RotationBack();
        PhoneControlMgr.Instance.SetActiveToF(true);
    }
    public void SetPlant(GameObject targetPlantPrefab,bool isConsumingSun = true)//在鼠标点击位置中下对应植物
    {
        if (targetPlantPrefab == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1 << 6))
        {
            Grid grid = hit.collider.gameObject.GetComponent<Grid>();
            Plants p = SelectedPlant.GetComponent<Plants>();
            if ((grid.IsEmpty() == true) && grid.IsBlockOccpuied() == false && grid.IsFitForPlant(p.type))
            {

                SetPlantsOnGrid(grid, targetPlantPrefab);

                TargetCard.OnPlant();

                if (CardSlot.Instance != null&&isConsumingSun)
                    CardSlot.Instance.SubSunCount(ResourceSystem.Instance.GetPlants(p.type).Consume);
            }
            EmptySelectedPlant();
        }
    }
    public void SetRandomPlant()
    {
        
            GameObject randomPlantPrefab = ResourceSystem.Instance.GetPlants((PlantsType)Random.Range(0, System.Enum.GetValues(typeof(PlantsType)).Length)).prefab;
            SetPlant(randomPlantPrefab,false);
    }
    public void SetRandomEnemy()
    {
        if (SelectedPlant == null) return;
        EnemyManager.Instance.CreateEnemyAtMousePos((EnemyType)Random.Range(0, System.Enum.GetValues(typeof(EnemyType)).Length), 3);
        TargetCard.OnPlant();
        EmptySelectedPlant();
    }

    private Plants SetPlantsOnGrid(Grid grid, GameObject Selectedplant, bool highLight = true)//在指定的GRID种植特定植物，并设定两者的引用
    {
        GameObject plant = Instantiate(Selectedplant, transform);
        Plants plantScr = plant.GetComponent<Plants>();
        plant.transform.position = grid.transform.position;
        if (AllPlantsHPtoOne == true)
        {
            plantScr.Health = 1;
        }
        plantScr.SetAttackSpeed(AttackSpeedAdder);
        plantScr.SetNowDamage(DamageAdder);

        grid.OnPlant(plant, highLight);



        //粒子效果
        GameObject particles = ObjectPool.Instance.GetObject(PlantParticles);
        particles.transform.position = grid.transform.position;
        return plant.GetComponent<Plants>();

    }

    public bool IsSelectedPlant()//是否已经选中植物
    {
        if (SelectedPlant != null)
            return true;
        return false;
    }
    public void RemoveAllPlants()
    {
        EmptySelectedPlant();
        GridManager.Instance.DeleteAllPlants();
    }

}
