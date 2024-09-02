using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private bool isEmpty = true;
    private bool isBlockOccupied;
    public Plants corePlants { get; set; }
    public bool UnAble;
    public bool isPlantPotGrid;
    public Plants potPlant;
    public GridType gridType = GridType.normal;
    private bool isLoaded;
    public bool IsWater;
    public bool IsInfectedMarked { get; set; }//如果才被感染，则不会进行下一次感染行为
    
    private ChunkGroup group;
    public GridType gridType_origin { get; private set; }
    private void Start()
    {
        EventMgr.Instance.AddEventListener("OnBlockPut", ()=> {
           
            CheckBlocks();
            CheckPlant();
        });
        Invoke("CheckPlant", 0.1f);
        if (!isLoaded && !isPlantPotGrid)
        {
            gridType_origin = gridType;
            if (gridType == GridType.normal)
                ChangeNearGrassColor(new Color(0.4f, 1, 0, 1));
            if (gridType == GridType.blood)
                ChangeNearGrassColor(Color.red);
        }

    }

    public ChunkGroup GetTargetChunkGroup()
    {
        return group;
    }
    public void Infect()
    {
        if (IsInfectedMarked) return;
        if (gridType != GridType.blood)return;
        int rand = Random.Range(0, 4);
        Ray ray = new Ray(transform.position + Vector3.down * 1.2f + Offset(rand), Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2.3f, 1 << 6))
        {
            Grid g = hit.collider.GetComponent<Grid>();
            if (g.IsInfectable() == true)
            {
                g.IsInfectedMarked = true;
                g.Transfer2();
            }
        }

    }


    private Vector3 Offset(int i)
    {
        switch (i)
        {
            case 0:
                return new Vector3(-2, 0, 0);
            case 1:
                return new Vector3(2, 0, 0);

            case 2:
                return new Vector3(0, 0, -2);
            case 3:
                return new Vector3(0, 0, 2);
            default:
                return Vector3.zero;
        }

    }
    public bool IsInfectable()
    {
        if (isPlantPotGrid == true)
        {
            return false;
        }

        if (gridType == GridType.normal || gridType == GridType.Stone)
        {
            return true;
        }
        return false;
    }
    public void Set(GridType type, GridType type_o, bool isWater)
    {
        this.IsWater = isWater;
        isLoaded = true;
        if (type == type_o)
        {
            gridType = type;

        }
        else
        {
            if (type == GridType.blood)
            {
                Transfer2();

            }
            else if (type_o == GridType.blood)
            {
                Transfer1();
            }
        }
        gridType_origin = type_o;

    }

    /// <summary>
    /// 净化
    /// </summary>
    public void Transfer1()
    {

        if (isPlantPotGrid)
        {
            return;
        }
        int startx = (int)transform.position.x - 1;
        int startz = (int)transform.position.z - 1;
        int y = (int)transform.position.y - 1;
        int stoneValue = 0;
        for (int i = startx; i < startx + 2; i++)
        {

            for (int j = startz; j < startz + 2; j++)
            {

                BlockType t = WorldManager.Instance.GetType(i, y, j);
                group = WorldManager.Instance.ReplaceABlock_Buffer(i, y, j, GridManager.Instance.GetMappingBlockType_1(t));
                if (GridManager.Instance.GetMappingBlockType_1(t) == BlockType.stone)
                {
                    stoneValue++;
                }
            }
        }
        if (gridType != GridType.UnPlantable)
        {
            gridType = GridType.normal;
            if (stoneValue > 2)
            {
                gridType = GridType.Stone;
            }
        }
        ChangeNearGrassColor(new Color(0.4f, 1, 0, 1));
        

    }
    /// <summary>
    /// 污染
    /// </summary>
    public void Transfer2()
    {
        if (isPlantPotGrid)
        {
            return;
        }
        
        int startx = (int)transform.position.x - 1;
        int startz = (int)transform.position.z - 1;
        int y = (int)transform.position.y - 1;
        for (int i = startx; i < startx + 2; i++)
        {

            for (int j = startz; j < startz + 2; j++)
            {
                BlockType t = WorldManager.Instance.GetType(i, y, j);
                group = WorldManager.Instance.ReplaceABlock_Buffer(i, y, j, GridManager.Instance.GetMappingBlockType_2(t));
            }
        }
        if (gridType != GridType.UnPlantable)
        {
            gridType = GridType.blood;
        }
        ChangeNearGrassColor(Color.red);
        
    }

    public void ChangeNearGrassColor(Color color)
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Grass");
        foreach (GameObject item in all)
        {
            if (Mathf.Abs(item.transform.position.x - transform.position.x) < 1 &&
                Mathf.Abs(item.transform.position.z - transform.position.z) < 1)
            {
                Thorn thorn = item.transform.parent.GetComponent<Thorn>();
                if (thorn != null)
                {

                    if (color.r > 0.8f)
                    {
                        thorn.TurnBlood();
                    }
                    else if (color.g > 0.8f)
                    {
                        thorn.TurnPure();
                    }
                }


                Renderer render = item.GetComponent<MeshRenderer>();
                render.material.SetColor("_Color", color);
            }
        }
        GameObject[] waters = GameObject.FindGameObjectsWithTag("Water");
        foreach (GameObject witem in waters)
        {
            if (Mathf.Abs(witem.transform.position.x - transform.position.x) < 1 &&
                Mathf.Abs(witem.transform.position.z - transform.position.z) < 1)
            {
                Water w = witem.GetComponent<Water>();
                if (w != null)
                {

                    if (color.r > 0.8f)
                    {

                        w.TurnBlood();
                    }
                    else if (color.g > 0.8f)
                    {
                        w.TurnPure();
                    }
                }
            }
        }
    }


    public void CheckPlant()//游戏初始化时检测上方是否有植物
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3, 1 << 9))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                isEmpty = false;
                Plants plant = hit.collider.gameObject.GetComponent<Plants>();
                plant.targetGrid = this;
                corePlants = plant;
            }
        }
    }
    public void CheckBlocks()
    {
        Vector3 a = transform.position + new Vector3(-0.5f, 5, 0.5f);
        Vector3 b = transform.position + new Vector3(0.5f, 5, 0.5f);
        Vector3 c = transform.position + new Vector3(-0.5f, 5, -0.5f);
        Vector3 d = transform.position + new Vector3(0.5f, 5, -0.5f);
        Vector3[] rayorigins = new Vector3[4]
        {
            a,b,c,d
        };

      
        if (RayCheckToFindTarget(rayorigins, new Vector3(0, -1, 0), 5, 3,"Blocks"))
        {
            isBlockOccupied = true;
        }
        else
        {
            isBlockOccupied = false;
        }
    }
    private bool RayCheckToFindTarget(Vector3[] poses, Vector3 dir, float length, int layer,string tag)//多条射线检测
    {
        Ray[] ray = new Ray[poses.Length];
        for (int i = 0; i < poses.Length; i++)
        {
            ray[i] = new Ray(poses[i], dir);
        }
        RaycastHit hit;
#if UNITY_EDITOR
        for (int i = 0; i < ray.Length; i++)
        {
            Debug.DrawLine(poses[i], poses[i] + dir.normalized * length);
        }
#endif
        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, length, 1 << layer))
            {
                if(hit.collider.CompareTag(tag))
                return true;
            }
        }
        return false;
    }
    public void OnPlant(GameObject targetplant, bool IsHighLight)//中下植物时执行一次
    {
        corePlants = targetplant.GetComponent<Plants>();
        corePlants.targetGrid = this;
        isEmpty = false;

        if (IsHighLight == true)
            GridManager.Instance.GridHighLightMgr(transform.position);

        //GridHighLight();
    }
    public void BindPlant(Plants targetPlant)
    {
        if (!isEmpty) return;

        isEmpty = false;
        corePlants = targetPlant;
        corePlants.targetGrid = this;

    }

    public void HighLighted()//单个GRID形成光标
    {
        GameObject g = ObjectPool.Instance.GetObject(GridManager.Instance.HighLightGizmos);
        g.transform.position = transform.position + new Vector3(0, 0.1f, 0);
    }
    public void RemovePlantBind()//解除绑定
    {
        if (corePlants != null)
        {
            corePlants.targetGrid = null;
            corePlants = null;
            isEmpty = true;
        }
    }
    public void RemovePlant()
    {
        if (corePlants != null)
        {
            Destroy(corePlants.gameObject);
            corePlants = null;
            isEmpty = true;
        }
    }
    public bool IsEmpty()
    {
        return isEmpty;
    }
    public bool IsBlockOccpuied()
    {
        if (UnAble == true)
        {
            return true;
        }
        else
            return isBlockOccupied;
    }
    public bool IsWaterPlant(PlantsType type)
    {
        if (type == PlantsType.Octopus
            || type == PlantsType.LotusLeaf)
        {
            return true;
        }
        return false;
    }
    public bool IsFitForPlant(PlantsType type)
    {
        if (IsWater == true)
        {
            return IsWaterPlant(type);
        }
        else
        {
            if (IsWaterPlant(type) == true)
            {
                return false;
            }
        }
        if (gridType == GridType.Stone && type == PlantsType.flowerPot)
        {
            return true;
        }
        else if (gridType == GridType.blood)
        {
            return true;
        }
        else if (gridType == GridType.normal)
        {
            return true;
        }
        return false;
    }


}
public enum GridType
{
    normal,
    Stone,
    blood,
    UnPlantable,

}
