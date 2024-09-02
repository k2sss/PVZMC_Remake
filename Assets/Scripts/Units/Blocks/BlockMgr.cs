using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : BaseManager<BlockManager>
{

    private GameObject SelectOutLine;
    public bool IsGizomos { get; private set; }
    private Transform PlayerTransform;
    public FunctionalBlock target;
    private bool IsDiged;
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50, 1 << 3))
        {
            if (hit.collider.gameObject.CompareTag("Blocks"))
            {
                FunctionalBlock f = hit.collider.GetComponent<FunctionalBlock>();
                target = f;
            }
            else
            {
                target = null;
            }
        }
        else
        {
            target = null;
        }




        if (IsGizomos == true)
        {
            SelectOutLine.transform.position = GetBlockPos();
        }
        if (target != null)
        {

                if (InputMgr.GetMouseButton(0) && WithInRange(target.transform.position)&& PhoneControlMgr.PhoneControl == false
                    ||(PhoneControlMgr.PhoneControl == true&&InputMgr.GetMouseButton(0) && WithInRange(target.transform.position)&&!PlayerMoveController.Instance.IsMove()))
                {
                    if (IsDiged == false)
                        OnDigStart();
                    PlayerMoveController.Instance.FaceToward(target.transform.position - PlayerMoveController.Instance.transform.position);
                    if(target.CanDig == true)
                    target.CauseDamage(8 * Time.deltaTime, PlayerItemManager.Instance.digtype, PlayerItemManager.Instance.diglevel, PlayerItemManager.Instance.digmultiplier, 1, 1);
                }
                else
                {
                    if (IsDiged == true)
                        OnDigExit();
                }
            
           
        }
        else
        {
            if (IsDiged == true)
                OnDigExit();
        }

    }

    public void OnDigStart()
    {
        IsDiged = true;
        PlayerItemManager.Instance.animator.SetBool("Dig", true);
        PlayerMoveController.Instance.IsBreakingBlock = true;
        PlayerMoveController.Instance.FreeToward = true;
    }
    public void OnDigExit()
    {
        IsDiged = false;
        PlayerItemManager.Instance.animator.SetBool("Dig", false);
        PlayerMoveController.Instance.IsBreakingBlock = false;
        PlayerMoveController.Instance.FreeToward = false;
    }

    private void Init()
    {
        GameObject G = FileLoadSystem.ResourcesLoad<GameObject>("block/Block_SelectLine");
        SelectOutLine = Instantiate(G);
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void EnableGizmos()
    {
        IsGizomos = true;
        SelectOutLine.SetActive(true);
    }
    public void DisableGizmos()
    {
        IsGizomos = false;
        SelectOutLine.SetActive(false);
    }
    public bool WithInRange(Vector3 pos)//判断是否在玩家范围内
    {
        Vector3 Dir = pos - PlayerTransform.position;
        if (Dir.magnitude <= 6)
        {
            return true;
        }

        return false;
    }
    public FunctionalBlock PutABlock(FunctionalBlockType type, Vector3 pos)
    {
        if (pos != Vector3.zero)
        {
            GameObject block = Instantiate(ResourceSystem.Instance.GetBlock(type).prefab, transform);
            block.transform.position = pos;
            FunctionalBlock f = block.GetComponent<FunctionalBlock>();
            f.info = new BlockSaveInfo(pos, type);
            MonoController.Instance.Invoke(0.01f, () => EventMgr.Instance.EventTrigger("OnBlockPut"));
            return f;
        }
        return null;
    }

    public void Remove(BlockSaveInfo info, GameObject thisobject)
    {
        Destroy(thisobject);
        MonoController.Instance.Invoke(0.01f, () => EventMgr.Instance.EventTrigger("OnBlockPut"));
        return;
    }
    public Vector3Int GetPlaceBlockPos_World()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50, 1 << 3))
        {
            Vector3Int v3int = placePos(hit.point, 1);
            int x = v3int.x;
            int y = v3int.y;
            int z = v3int.z;
            return new Vector3Int(x, y, z);
        }
        return new Vector3Int(0, 0, 0);
    }//获得将要放置方块位置的 世界坐标

    public Vector3 GetBlockPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50, 1 << 3))
        {
            //if (hit.collider.CompareTag("Blocks"))
            //{
            //    return new Vector3(0, 0, 0);
            //}
            Vector3Int v3int = placePos(hit.point, 1);
            int x = v3int.x;
            int y = v3int.y;
            int z = v3int.z;
            return new Vector3(x + 0.5f, y, z + 0.5f);
        }
        return new Vector3(0, 0, 0);
    }

    private Vector3Int placePos(Vector3 point, int backwards)
    {

        Vector3 Dir = backwards * ((Camera.main.transform.position - point).normalized) / 10;
        Vector3Int PlacePos = new Vector3Int((int)(Dir.x + point.x), (int)(Dir.y + point.y), (int)(Dir.z + point.z));
        return PlacePos;
    }

}
[System.Serializable]
public class BlockSaveInfo
{
    public Vector3 pos;
    public FunctionalBlockType type;
    public BlockSaveInfo(Vector3 pos, FunctionalBlockType type)
    {
        this.pos = pos;
        this.type = type;
    }
}