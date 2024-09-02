using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : BaseManager<EnemyManager>
{
    public Transform[] CreatePositions;
    public float CreateXPosOffSet;
    public List<EnemyCreateInfo> enemycreateInfos = new List<EnemyCreateInfo>();
    public GameObject[] enemys;
    public float k = 1;//б��
    public float b = 1;//��ʼ����
    public float BigWaveMultiplier = 1.5f;
    public float[] rowWeights;//ÿһ�����ɹ����Ȩ��
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
    public bool CheckEnemys()//����ֳ��Ƿ��й���
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
        Debug.LogError("�޷������λ�ô�������");
        return null;
    }
    public Enemy CreateEnemy(EnemyType type, Vector3 pos)
    {
        GameObject prefab = ResourceSystem.Instance.GetEnemy(type).prefab;
        //��������Ķ���ΪBOSS��λ���򴴽���ͨ��ʬ����ֹ���������ʱˢ��BOSS
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
    }//��ָ��λ�ô������
    public Enemy CreateEnemyInRandomRow(EnemyType type)//����ڸ������ɹ���
    {
        int row = 0;//Ҫ��������һ��
        while (true)//��ô����Ŀ�ģ�ʹ���ɵĹ��ﾡ���ܷ�ɢ
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
    public Enemy CreateEnemyInRandomRow(EnemyType type,float XoffSet)//����ڸ������ɹ���
    {
        int row = 0;//Ҫ��������һ��
        while (true)//��ô����Ŀ�ģ�ʹ���ɵĹ��ﾡ���ܷ�ɢ
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

    public void CreateNextWaveEnemys()//����һ����ʬ
    {
        
        RowWeightsInit();
       
        int a = GetTotalNum(LevelManager.Instance.nowWave);
       
        //�����ս��  --->  ���� ��ʬȨ�� �鿨 --->
        //��ȡ���ϲ��������ĵ���
        List<EnemyCreateInfo> enemy = new List<EnemyCreateInfo>();
       
        foreach (EnemyCreateInfo info in enemycreateInfos)
        {
            if (LevelManager.Instance.nowWave >= info.AppearWave)
            {
                enemy.Add(info);
            }
        }
        

        //����Ȩ�������콩ʬ
        RefreshEnemyWeights(ref enemy);//ˢ��Ȩ��
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
    public int GetTotalNum(int num)//��õ�ǰ��������ս��
    {
      
        float t = k * num + b;
        
        if (LevelManager.Instance.CompareBigWave(num))
        {
            t *= BigWaveMultiplier;
        }
        int total = (int)t;//��ս��
        return total;
    }

    public void RefreshEnemyWeights(ref List<EnemyCreateInfo> enemy)//��enemy�Ľ�ʬȨ��ˢ��
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

    public int GetZombieTotalWeights(List<EnemyCreateInfo> enemy)//��ù������Ȩ��
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
