using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelDataSpace;

    [System.Serializable]
    public class LevelData
    {
        public string LevelName;
    
        public degreetype LevelType;
    
        public bool canLoad;
        public ItemInfo[] templeInventoryItems;
        public ItemInfo[] templeArmorItems;
        public int SunCount;//Ñô¹âÊý
        public PlayerInfo playerInfo;
        public List<EnemyInfo> enemyInfos = new List<EnemyInfo>();
        public List<PlantsInfo> plantsinfos = new List<PlantsInfo>();
        public List<CardsInfo> cardsinfos = new List<CardsInfo>();
        public List<ITEM> items = new List<ITEM>();
        public LevelInfo levelInfo = new LevelInfo();
        public List<BlockData> blocksinfos;
    public List<GridData> gridinfos;
        
    public List<Vector3> mineCarPos;
    }
namespace LevelDataSpace
{
    [System.Serializable]
    public class EnemyInfo
    {
        public Vector3 pos;
        public EnemyType type;
        public float health;
        public float ArmorHealth;
        public bool MaybeUsebool1;

    }
    [System.Serializable]
    public class PlantsInfo
    {
        public PlantsType type;
        public Vector3 Pos;
        public float Health;
        public bool MaybeUseBool;
        public float MaybeUseTimer;
        public bool MaybeUseBool2;
        public float MaybeUseTimer2;

    }
    [System.Serializable]
    public class CardsInfo
    {
        public PlantsType plantType;
        public bool GetOnce;
        public float nowCD;
        public bool CdOff;
    }
    [System.Serializable]
    public class PlayerInfo
    {
        public Vector3 transformRight;
        public Vector3 WorldPosition;
        public float health;
        public int Hunger;
        public int Hunger_over;
        public int Exp;
        public int ExpLevel;
        public float maxHealth;
    }
    [System.Serializable]
    public class LevelInfo
    {
        public int nowWave;
        public float WaveTimer;
        public bool StopCountTimer;
    }
    [System.Serializable]
    public class ITEM
    {
        public Vector3 pos;
        public Vector3 EulerAngle;
        public ItemInfo info;
    }
    [System.Serializable]
    public class BlockData
    {
        public FunctionalBlockType type;
        public float BlockHealth;
        public Vector3 position;
        public int info1;
    }
    [System.Serializable]
    public class GridData
    {
        public GridType gtype_o;
        public GridType gtype;
        public Vector3 position;
        public bool isWater;
        public GridData(GridType gtype,GridType gtype_o, Vector3 position,bool isWater)
        {
            this.gtype = gtype;
            this.position = position;
            this.gtype_o = gtype_o;
            this.isWater = isWater; 
        }
    }
}
