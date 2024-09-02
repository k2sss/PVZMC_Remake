using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : BaseManager<GridManager>
{
    public int width = 5;
    public int length = 9;
    public GameObject GridPrefab;
    public Grid[] grids { get; set; }
    public GameObject HighLightGizmos;
    private float InfectTimer = 0;
    private bool isGameStart;

    void Start()
    {
        GetGrids();
        EventMgr.Instance.AddEventListener("GameStart", () => isGameStart = true);
    }
    private void Update()
    {

        if (!isGameStart) return;

        InfectTimer += Time.deltaTime;
        if (InfectTimer > 45)
        {
            InfectTimer = 0;
            for (int i = 0; i < grids.Length; i++)
            {
                grids[i].IsInfectedMarked = false;
            }
            for (int i = 0;i<grids.Length;i++)
            {
                grids[i].Infect();  
            }
            WorldManager.UpdateWorldInfo();
         
        }
    }
    public void ReUpdate()
    {
        for (int i = 0; i < grids.Length; i++)
        {
            if(!grids[i].IsEmpty())
            grids[i].CheckPlant();
        }
    }

    [ContextMenu("GridInit")]
    public void InsantiateGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i));
        }
        for (int i = 0; i < width; i++)
            for (int j = 0; j < length; j++)
            {
                GameObject grid = Instantiate(GridPrefab, transform);
                grid.transform.position = transform.position + new Vector3(1 + j * 2, 0, 1 + i * 2);
            }
    }
    public void GetGrids()
    {

        grids = new Grid[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            grids[i] = transform.GetChild(i).transform.GetComponent<Grid>();
        }
    }
    public void GridHighLightMgr(Vector3 pos)
    {


        foreach (Grid g in grids)
        {

            if (Mathf.Abs(g.transform.position.x - pos.x) < 0.2f || Mathf.Abs(g.transform.position.z - pos.z) < 0.2f)
            {
                g.HighLighted();
            }

        }


    }
    public void DeleteAllPlants()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(grids[i].corePlants.gameObject);
            grids[i].RemovePlantBind();
        }
    }
    public bool ThisPlaceIsOccpuied()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50, 1 << 6))
        {
            return !hit.collider.gameObject.GetComponent<Grid>().IsEmpty();
        }

        return false;
    }
    public float GetMaxX()
    {
        return transform.position.x + length * 2;
    }

    public float GetMinZ()
    {
        return transform.position.z;
    }
    public float GetMaxZ()
    {
        return transform.position.z + width * 2;
    }
    public Vector3 GetMaxPos()
    {
        return new Vector3(transform.position.x + length * 2,transform.position.y, transform.position.z + width * 2);
    }
    public float TransToGridZAxis(float Zaxis)
    {
        Zaxis = Mathf.Clamp(Zaxis, GetMinZ(), GetMaxZ());
        int lineNum = (int)((Zaxis - GetMinZ()) / 2);
        return GetMinZ() + lineNum * 2 + 1;
    }

    public BlockType GetMappingBlockType_1(BlockType atype)//¾»»¯
    {
        switch (atype)
        {
            case BlockType.fleshBlock:
                return BlockType.stone;
            case BlockType.fleshdirt:
                return BlockType.grass_block;
            case BlockType.fleshgrass:
                return BlockType.grass_block_light;
            case BlockType.fleshSand:
                return BlockType.Sand;
            case BlockType.fleshOre:
                return BlockType.stone;
            case BlockType.fleshBone1:
                return BlockType.grass_block;
            case BlockType.fleshBone2:
                return BlockType.grass_block_light;
            case BlockType.fleshBone3:
                return BlockType.grass_block;
            case BlockType.gloomyWood:
                return BlockType.oak_log;
            case BlockType.gloomyWodd_Leave:
                return BlockType.oak_leave_empty;
            default:
                return BlockType.grass_block;


        }
    }
    public BlockType GetMappingBlockType_2(BlockType atype)//ÑªÐÈ»¯
    {
        switch (atype)
        {
            case BlockType.stone:
                return BlockType.fleshBlock;
            case BlockType.Sand:
                return BlockType.fleshSand;
            case BlockType.grass_block:

                return BlockType.fleshdirt;
            case BlockType.grass_block_light:
                return BlockType.fleshgrass;
            case BlockType.oak_log:
                return BlockType.gloomyWood;
            case BlockType.oak_leave_empty:
                return BlockType.gloomyWodd_Leave;
            default:
                return BlockType.fleshdirt;
        }
    }
}