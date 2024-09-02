using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LevelDataSpace;
public class SaveManager : BaseManager<SaveManager>
{
    private Transform CardParent;
    
    private bool CanSave = true;
    private void Start()
    {
        Init();
        EventMgr.Instance.AddEventListener("GameWin", () => MySystem.Instance.nowLeveldata.canLoad = false);
        EventMgr.Instance.AddEventListener("GameWin", MySystem.Instance.SaveNowLevelData);
        EventMgr.Instance.AddEventListener("GameWin", ()=>CanSave = false);
        EventMgr.Instance.AddEventListener("GameOver", () => MySystem.Instance.nowLeveldata.canLoad = false);
        EventMgr.Instance.AddEventListener("GameOver", MySystem.Instance.SaveNowLevelData);
        EventMgr.Instance.AddEventListener("GameOver", () => CanSave = false);
        
        EventMgr.Instance.AddEventListener("ChangeWave", ()=>MonoController.Instance.InvokeUnScaled(0.01f,SaveAll));
        Invoke("LoadAll", 0.01f);
    }
    public void Init()
    {
        if (CardSlot.Instance != null)
            CardParent = CardSlot.Instance.CardsParent;
   
    }
    public void SaveAll()
    {
        if (CanSave == true)
        {
            
            LevelData leveldata = new LevelData();
            
            leveldata.canLoad = true;
            //Debug.Log("2");
            if (CardSlot.Instance != null)
            leveldata.SunCount = CardSlot.Instance.SunCount;
            // Debug.Log("2.1");
            leveldata.playerInfo = ScanPlayerInfo();
           // Debug.Log("2.2");
            leveldata.plantsinfos = ScanPlants();
            //Debug.Log("2.3");
            leveldata.cardsinfos = ScanCards();
            //Debug.Log("2.4");
            leveldata.enemyInfos = ScanEnemy();
            leveldata.levelInfo = ScanLevelInfo();
            leveldata.items = ScanAllItemOnGround();
           // Debug.Log("3");
            leveldata.blocksinfos = ScanAllBlocks();

            leveldata.mineCarPos = ScanMineCar();

            leveldata.gridinfos = ScanGrid();

            leveldata.LevelName = MySystem.Instance.nowUserData.levelName;
           
            leveldata.LevelType = MySystem.Instance.nowUserData.LevelType;
           
            if (InventoryManager.Instance != null)
            {
                leveldata.templeInventoryItems = InventoryManager.Instance.InventoryItems;
               
                Transform slotParent = InventoryManager.Instance.transform.Find("Inventory/Inventory_Main/ArmorSlots");
                leveldata.templeArmorItems = new ItemInfo[slotParent.childCount];
                for (int i = 0; i < slotParent.childCount; i++)
                {
                    ArmorSlot slot = slotParent.GetChild(i).gameObject.GetComponent<ArmorSlot>();
                    leveldata.templeArmorItems[i] = new ItemInfo(slot.info.type,slot.info.Count,slot.info.Durability);
                }
            }
                
            MySystem.Instance.nowLeveldata = leveldata;
            MySystem.Instance.SaveNowLevelData();
            //Debug.Log("已保存");
        }


    }//保存本关卡所有存档
    public void LoadAll()
    {

        LevelData newleveldata;
        newleveldata = MySystem.Instance.nowLeveldata;
        if (MySystem.CanLoadLevelData())
        {
            EventMgr.Instance.EventTrigger("GameStart");
            // Debug.Log("0");
            CardSlot.Instance.SetSunCount(newleveldata.SunCount);
            // Debug.Log("1");
            LoadLevelInfo(newleveldata);
            // Debug.Log("2");
            SetPlayerInfo(newleveldata);
            // Debug.Log("3");
            LoadGrid(newleveldata);
            LoadAllPlants(newleveldata);
            // Debug.Log("4");
            LoadEnemy(newleveldata);
            //Debug.Log("5");
            LoadCards(newleveldata);

            LoadAllBlocks(newleveldata);
            //Debug.Log("6");
            LoadAllItemOnGround(newleveldata);
            // Debug.Log("8");
            LoadMineCar(newleveldata);

           
           
            for (int i = 0; i < newleveldata.templeInventoryItems.Length; i++)
            {
                InventoryManager.Instance.InventoryItems[i] = newleveldata.templeInventoryItems[i].ShallowClone();
            }
            Transform slotParent = InventoryManager.Instance.transform.Find("Inventory/Inventory_Main/ArmorSlots");

            for (int i = 0; i < newleveldata.templeArmorItems.Length; i++)
            {
                ArmorSlot slot = slotParent.GetChild(i).gameObject.GetComponent<ArmorSlot>();
                slot.info = newleveldata.templeArmorItems[i].ShallowClone();
            }
            
            EventMgr.Instance.EventTrigger("UpdateArmor");
           
            //Debug.Log("9");
            InventoryManager.Instance.UpdateItemDic();
            // Debug.Log("10");
            EventMgr.Instance.EventTrigger("OnChangeInventory");
            //Debug.Log("11");

        }

    }//读取本关存档
    private PlayerInfo ScanPlayerInfo()
    {
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        PlayerInfo go = new PlayerInfo();
        go.health = playerHealth.Health;
        go.transformRight = playerHealth.transform.right;
        go.WorldPosition = playerHealth.transform.position;
        go.Hunger = PlayerHunger.Instance.Hunger;
        go.Hunger_over = PlayerHunger.Instance.Hunger_over;
        go.Exp = PlayerExp.Instance.Exp;
        go.ExpLevel = PlayerExp.Instance.Level;
        go.maxHealth = playerHealth.MaxHealth;
        return go;
    }//获得玩家的当前信息
    private void SetPlayerInfo(LevelData data)
    {
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        playerHealth.Health = data.playerInfo.health;
        if(data.playerInfo.maxHealth!=0)
        playerHealth.MaxHealth = data.playerInfo.maxHealth;
        playerHealth.transform.position = data.playerInfo.WorldPosition;
        playerHealth.transform.right = data.playerInfo.transformRight;
        PlayerHunger.Instance.Hunger = data.playerInfo.Hunger;
        PlayerHunger.Instance.Hunger_over = data.playerInfo.Hunger_over;


    }//将存档数据赋值给玩家

    private List<PlantsInfo> ScanPlants()
    {
        List<PlantsInfo> list = new List<PlantsInfo>();
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plants");
        for (int i = 0; i < plants.Length; i++)
        {

                Plants p = plants[i].GetComponent<Plants>();
                PlantsInfo info = new PlantsInfo();
                info.Pos = p.transform.position;
                info.type = p.type;
                info.Health = p.Health;
                info.MaybeUseBool = p.MaybeUseBool;
                info.MaybeUseTimer = p.MaybeUseTimer;
                info.MaybeUseTimer2 = p.MaybeUseTimer2;
                info.MaybeUseBool2 = p.MaybeUseBool2;
                list.Add(info);
        }
        return list;
    }//获取当前所有在场植物的信息
    private void LoadAllPlants(LevelData data)
    {

        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plants");
        for (int i = 0; i < plants.Length; i++)
        {
            Destroy(plants[i]);
        }
        for (int i = 0; i < data.plantsinfos.Count; i++)
        {
            PlantsInfo info = data.plantsinfos[i];
            //Plants p = PlantManager.Instance.SetPlantsOnGrid(GridManager.Instance.grids[info.gridx, info.gridy], ResourceSystem.Instance.GetPlants(info.type).prefab, false); ;
            Plants p =  Instantiate( ResourceSystem.Instance.GetPlants(info.type).prefab).GetComponent<Plants>();
            p.transform.position = info.Pos;
            p.Health = info.Health;
            p.MaybeUseBool = info.MaybeUseBool;
            p.MaybeUseTimer = info.MaybeUseTimer;
            p.MaybeUseTimer2 = info.MaybeUseTimer2;
            p.MaybeUseBool2 = info.MaybeUseBool2;

        }

    }//种下存档中的植物

    private List<CardsInfo> ScanCards()
    {
        List<CardsInfo> list = new List<CardsInfo>();
        for (int i = 0; i < CardParent.childCount; i++)
        {
            CardsInfo info = new CardsInfo();
            Card c = CardParent.GetChild(i).gameObject.GetComponent<Card>();
            info.CdOff = c.CdOff;
            info.GetOnce = true;
            info.plantType = c.type;
            info.nowCD = c.nowCD;
            list.Add(info);
        }
        return list;
    }
    private void LoadCards(LevelData data)
    {
        for (int i = 0; i < data.cardsinfos.Count; i++)
        {
            CardSlot.Instance.AddAnewCardInSlot(data.cardsinfos[i]);

        }
    }

    private List<EnemyInfo> ScanEnemy()
    {
        List<EnemyInfo> list = new List<EnemyInfo>();
        GameObject[] AllEnemys = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < AllEnemys.Length; i++)
        {
            Enemy e = AllEnemys[i].GetComponent<Enemy>();
            EnemyInfo info = new EnemyInfo();
            info.health = e.Health;
            info.pos = e.transform.position;
            info.type = e.type;
            info.ArmorHealth = e.ArmorHealth;
            info.MaybeUsebool1 = e.MaybeUsebool1;
            list.Add(info);
        }
        return list;
    }

    private void LoadEnemy(LevelData data)
    {
        for (int i = 0; i < data.enemyInfos.Count; i++)
        {
            EnemyInfo info = data.enemyInfos[i];
            Enemy e = EnemyManager.Instance.CreateEnemy(info.type, info.pos);
            e.Health = info.health;
            e.ArmorHealth = info.ArmorHealth;
            e.MaybeUsebool1 = info.MaybeUsebool1;
        }
    }

    private LevelInfo ScanLevelInfo()
    {
        LevelInfo info = new LevelInfo();
        info.nowWave = LevelManager.Instance.nowWave;
        info.WaveTimer = LevelManager.Instance.WaveTimer;
        info.StopCountTimer = LevelManager.Instance.isStopTimer;
        return info;
    }
    private void LoadLevelInfo(LevelData data)
    {
        LevelManager.Instance.nowWave = data.levelInfo.nowWave;
        LevelManager.Instance.WaveTimer = data.levelInfo.WaveTimer;
        LevelManager.Instance.isStopTimer = data.levelInfo.StopCountTimer;
    }

    private List<ITEM> ScanAllItemOnGround()
    {
        List<ITEM> list = new List<ITEM>();
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < items.Length; i++)
        {
            ITEM go = new ITEM();
            go.pos = items[i].transform.position;
            go.EulerAngle = items[i].transform.eulerAngles;
            go.info = items[i].GetComponent<Item>().info;
            list.Add(go);
        }
        return list;
    }
    private List<BlockData> ScanAllBlocks()
    {
       
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Blocks");

        List<BlockData> blockdatas = new List<BlockData>();
        for (int i = 0; i < blocks.Length; i++)
        {
            BlockData data = new BlockData();
            data.position = blocks[i].transform.position;
            FunctionalBlock f = blocks[i].GetComponent<FunctionalBlock>();
            data.type = f.Btype;
            data.BlockHealth = f.health;
            data.info1 = f.info_int_1;
            blockdatas.Add(data);
        }
        
        return blockdatas;

    }
    private void LoadAllBlocks(LevelData data)
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Blocks");
        for (int i = 0; i < blocks.Length; i++)
        {
            Destroy(blocks[i]);
        }
        for (int i = 0; i < data.blocksinfos.Count; i++)
        {
            BlockData blockdata = data.blocksinfos[i];
            FunctionalBlock f = BlockManager.Instance.PutABlock(blockdata.type, blockdata.position);
            f.health = blockdata.BlockHealth;
            f.info_int_1 = blockdata.info1;
        }
    }
    private void LoadAllItemOnGround(LevelData data)
    {
        for (int i = 0; i < data.items.Count; i++)
        {
            GameObject go = Instantiate(ResourceSystem.Instance.GetItem(data.items[i].info).prefab);
            go.transform.position = data.items[i].pos;
            go.transform.eulerAngles = data.items[i].EulerAngle;
            go.GetComponent<Item>().info = data.items[i].info;

        }
    }

    private List<Vector3> ScanMineCar()
    {
        GameObject[] allMineCars = GameObject.FindGameObjectsWithTag("MineCar");
        List<Vector3> cars = new List<Vector3>();
        for (int i = 0; i < allMineCars.Length; i++)
        {
            cars.Add(allMineCars[i].transform.position);
        }
        return cars;
    }
    private void LoadMineCar(LevelData data)
    {
        GameObject[] allMineCars = GameObject.FindGameObjectsWithTag("MineCar");
        for (int i = 0; i < allMineCars.Length; i++)
        {
            Destroy(allMineCars[i]);
        }
        GameObject carPrefab = FileLoadSystem.ResourcesLoad<GameObject>("prefab/MineCar");
        for (int i = 0; i < data.mineCarPos.Count; i++)
        {
            GameObject g = Instantiate(carPrefab);
            g.transform.position = data.mineCarPos[i];
        }
    }

    private List<GridData> ScanGrid()
    {
        GameObject[] allGrid = GameObject.FindGameObjectsWithTag("Grid");
        List<GridData> go = new List<GridData>();
        for (int i = 0; i < allGrid.Length; i++)
        {
            Grid g = allGrid[i].GetComponent<Grid>();
            if (g.isPlantPotGrid == false)
            {
                go.Add(new GridData(g.gridType,g.gridType_origin,g.transform.position,g.IsWater));
            }
            
        }
        return go;
    }
    private void LoadGrid(LevelData data)
    {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Grid");
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].GetComponent<Grid>().isPlantPotGrid == false)
            Destroy(all[i]);
        }
        GameObject Prefab = FileLoadSystem.ResourcesLoad<GameObject>("prefab/Grid");
        for (int i = 0; i < data.gridinfos.Count; i++)
        {
            GameObject g = Instantiate(Prefab,GridManager.Instance.transform);
            g.transform.position = data.gridinfos[i].position;
            g.GetComponent<Grid>().Set(data.gridinfos[i].gtype, data.gridinfos[i].gtype_o, data.gridinfos[i].isWater);
            
            
           
        }
        MonoController.Instance.Invoke(0.01f,()=> GridManager.Instance.GetGrids())
       ;
    }
    
}
