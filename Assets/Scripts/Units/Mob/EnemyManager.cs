using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : BaseManager<EnemyManager>
{
    public Transform[] CreatePositions;
    public float CreateXPosOffSet;
    public List<EnemyCreateInfo> enemycreateInfos = new List<EnemyCreateInfo>();
    public GameObject[] enemys;
    public float k = 1;//斜率
    public float b = 1;//初始数量
    public float BigWaveMultiplier = 1.5f;
    public float[] rowWeights;//每一行生成怪物的权重
    private float RowFadeMultiplier = 0.3f;
    private float RandomValue = 1.5f;
    public float Height;
    public float EnemyHealthMultiplier = 1;
    public float EnemySpeedAdder = 0;
    public float EnemyAttackSpeedAdder = 0;
    public float EnemyDamageAdder = 0;
    private List<GameObject> TempleEnemys = new List<GameObject>();
    private Queue<EnemyType> EnemyTypeQueue = new Queue<EnemyType>();
    private void Update()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void SummonEnemy_Temple()
    {
        RowWeightsInit();

        for (int i = 0; i < enemycreateInfos.Count; i++)
        {
            int rand = 0;
            if (enemycreateInfos[i].Weight >= 4000)
            {
                rand = Random.Range(2, 4);
            }
            else if (enemycreateInfos[i].Weight >= 3000)
            {
                rand = Random.Range(1, 3);
            }
            else
            {
                rand = 1;
            }
            for (int j = 0; j < rand; j++)
            {

                Enemy e = CreateEnemyInRandomRow(enemycreateInfos[i].type);
                e.transform.position += new Vector3(-1, -Height + 2, Random.Range(-2f, 1f));
                e.enabled = false;
                if (e.hurtConstraint != null)
                {
                    e.hurtConstraint.weight = 0;
                }
                TempleEnemys.Add(e.gameObject);


            }


        }


        EventMgr.Instance.AddEventListener("GameStart", DeleteAllTempleEnemy);

    }
    private void DeleteAllTempleEnemy()
    {
        for (int i = 0; i < TempleEnemys.Count; i++)
        {
            Destroy(TempleEnemys[i]);
        }
    }

    public void RowWeightsInit()
    {
        rowWeights = new float[CreatePositions.Length];
        for (int i = 0; i < rowWeights.Length; i++)
        {
            rowWeights[i] = 1000;
        }
    }
    public float GetRowTotalWeights()
    {
        float totalWeights = 0;

        for (int i = 0; i < rowWeights.Length; i++)
        {
            totalWeights += rowWeights[i];
        }

        return totalWeights;
    }
    public bool CheckEnemys()//检测现场是否有怪物
    {

        if (enemys.Length == 0)
        {
            return false;

        }
        else
        {

            return true;
        }

    }
    public Enemy CreateEnemyAtMousePos(EnemyType enemyType,int layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,50,1<<layer))
        {
           return CreateEnemy(enemyType, hit.point);
        }
        Debug.LogError("无法在鼠标位置创建敌人");
        return null;
    }
    public Enemy CreateEnemy(EnemyType type, Vector3 pos)
    {
        GameObject prefab = ResourceSystem.Instance.GetEnemy(type).prefab;
        //如果创建的对象为BOSS单位，则创建普通僵尸，防止随机出敌人时刷出BOSS
        if (prefab.GetComponent<Enemy>().IsBoss) prefab = ResourceSystem.Instance.GetEnemy(EnemyType.zombie).prefab;
        GameObject eObj = Instantiate(prefab);
        eObj.transform.position = pos;
        Enemy go = eObj.GetComponent<Enemy>();
        go.Health *= EnemyHealthMultiplier;
        go.SetNowSpeed(EnemySpeedAdder);
        go.SetAnimatorSpeed(EnemySpeedAdder);
        go.SetAttackSpeed(EnemyAttackSpeedAdder);
        go.SetNowDamage(EnemyDamageAdder);
        return go;
    }//在指定位置创造怪物
    public Enemy CreateEnemyInRandomRow(EnemyType type)//随机在各行生成怪物
    {
        int row = 0;//要生成在哪一行
        while (true)//这么做的目的：使生成的怪物尽可能分散
        {
            int t = Random.Range(0, rowWeights.Length);
            if (Random.Range(0, 1f) <= rowWeights[t] / GetRowTotalWeights())
            {
                row = t;
                rowWeights[t] *= RowFadeMultiplier;
                break;
            }
        }
        Vector3 pos = CreatePositions[row].position + new Vector3(Random.Range(-RandomValue, RandomValue), Height, 0);
        return CreateEnemy(type, pos);
    }
    public Enemy CreateEnemyInRandomRow(EnemyType type,float XoffSet)//随机在各行生成怪物
    {
        int row = 0;//要生成在哪一行
        while (true)//这么做的目的：使生成的怪物尽可能分散
        {
            int t = Random.Range(0, rowWeights.Length);
            if (Random.Range(0, 1f) <= rowWeights[t] / GetRowTotalWeights())
            {
                row = t;
                rowWeights[t] *= RowFadeMultiplier;
                break;
            }
        }
        Vector3 pos = CreatePositions[row].position + new Vector3(Random.Range(-RandomValue, RandomValue) + XoffSet, Height, 0);
        return CreateEnemy(type, pos);
    }

    public void CreateNextWaveEnemys()//创造一波僵尸
    {
        
        RowWeightsInit();
       
        int a = GetTotalNum(LevelManager.Instance.nowWave);
       
        //获得总战力  --->  根据 僵尸权重 抽卡 --->
        //抽取符合波数条件的敌人
        List<EnemyCreateInfo> enemy = new List<EnemyCreateInfo>();
       
        foreach (EnemyCreateInfo info in enemycreateInfos)
        {
            if (LevelManager.Instance.nowWave >= info.AppearWave)
            {
                enemy.Add(info);
            }
        }
        

        //根据权重来创造僵尸
        RefreshEnemyWeights(ref enemy);//刷新权重
        int createCount = 0;
        
        EnemyTypeQueue.Clear();
        while (createCount < a)
        {
            while (true)
            {
                int t = Random.Range(0, enemy.Count);
                if (Random.Range(0, 1f) <= (float)enemy[t].Weight / GetZombieTotalWeights(enemy))
                {
                    EnemyTypeQueue.Enqueue(enemy[t].type);
                    //Enemy templeEnemy = CreateEnemyInRandomRow(enemy[t].type);
                   // templeEnemy.transform.position = templeEnemy.transform.position + new Vector3(CreateXPosOffSet, 0, 0);
                    int temple = createCount + enemy[t].Consume;
                    if (temple <= a)
                    {
                        createCount = temple;
                        break;
                    }
                    else
                    {
                        // Destroy(templeEnemy.gameObject);
                        EnemyTypeQueue.Dequeue();
                    }
                }
            }
        }
        
        foreach (EnemyType t in EnemyTypeQueue)
        {
            MonoController.Instance.Invoke(Random.Range(0, 1f), () =>
            {
                Enemy e = CreateEnemyInRandomRow(t);
                e.transform.position = e.transform.position + new Vector3(CreateXPosOffSet, 0, 0);
            });
        }

    }
    public int GetTotalNum(int num)//获得当前波数的总战力
    {
      
        float t = k * num + b;
        
        if (LevelManager.Instance.CompareBigWave(num))
        {
            t *= BigWaveMultiplier;
        }
        int total = (int)t;//总战力
        return total;
    }

    public void RefreshEnemyWeights(ref List<EnemyCreateInfo> enemy)//将enemy的僵尸权重刷新
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            enemy[i].Weight += enemy[i].WeightAdd;
            if (enemy[i].Weight < 0)
            {
                enemy[i].Weight = 0;
            }
        }
    }

    public int GetZombieTotalWeights(List<EnemyCreateInfo> enemy)//获得怪物的总权重
    {
        int totalWeights = 0;
        for (int i = 0; i < enemy.Count; i++)
        {
            totalWeights += enemy[i].Weight;
        }
        return totalWeights;
    }

    public float GetMaxLinePosZ()
    {
        float[] rowsZ = new float[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            rowsZ[i] = gameObject.transform.GetChild(i).transform.position.z;
        }
        float go = rowsZ[0];
        for (int i = 1; i < rowsZ.Length; i++)
        {
            if (rowsZ[i] > go)
            {
                go = rowsZ[i];
            }
        }
        return go;
    }
    public float GetMinLinePosZ()
    {
        float[] rowsZ = new float[gameObject.transform.childCount];
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            rowsZ[i] = gameObject.transform.GetChild(i).transform.position.z;
        }
        float go = rowsZ[0];
        for (int i = 1; i < rowsZ.Length; i++)
        {
            if (rowsZ[i] <= go)
            {
                go = rowsZ[i];
            }
        }
        return go;
    }
}
[System.Serializable]
public class EnemyCreateInfo
{
    public int Consume = 1;
    public EnemyType type;
    public int AppearWave = 1;
    public int Weight = 4000;
    public int WeightAdd;


    public EnemyCreateInfo()
    {

    }
    public EnemyCreateInfo(int consume, EnemyType type, int appearWave, int weight, int weightAdd)
    {
        Consume = consume;
        this.type = type;
        AppearWave = appearWave;
        Weight = weight;
        WeightAdd = weightAdd;
    }
}
