using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Scriptable_Object_List
{
    public List<Item_Scriptable> itemlist = new List<Item_Scriptable>();
    public List<Enemy_Scriptable> enemylist = new List<Enemy_Scriptable>();
    public List<Plant_Scriptable> plantslist = new List<Plant_Scriptable>();
    public List<Block_Scriptable> blockList = new List<Block_Scriptable>();
    public List<Sprite_Scriptable> spritelist = new List<Sprite_Scriptable>();
    public List<Particles_Scriptable> particlelist = new List<Particles_Scriptable>();
    public List<Bullet_Scriptable> bulletlist = new List<Bullet_Scriptable>();
}
[System.Serializable]
public class Item_Scriptable
{
    public string myName;
    public ItemType type;
    public List<Sprite> AnimationSprites = new List<Sprite>();
    public float AnimationGapTime = 0.4f;
    public int MaxAmount = 64;
    public int MaxDurability = 1;
    public GameObject prefab;
    public List<ItemText> itemtexts = new List<ItemText>();
}
[System.Serializable]
public class Enemy_Scriptable
{
    public string myName;
    public EnemyType type;
    public float aimHeight = 1;
    public GameObject prefab;
    public AudioClip[] HurtSounds;
    public AudioClip[] IdleSounds;
    public AudioClip[] DeathSounds;
    public AudioClip[] AttackSounds;
    public List<EnemyDropItem> dropItems = new List<EnemyDropItem>();
}
[System.Serializable]
public class Plant_Scriptable
{
    public string myName;
    public string Description;
    public PlantsType type;
    public int Consume;
    public float CD;
    public Sprite sprite;
    public bool StartWithNoCD;
    public GameObject prefab;

}
[System.Serializable]

public class Buff_Scriptable
{
    public BuffType type;
    private bool _IsWorking;
    public Action onStartWork;
    public Action onStopWork;
    public bool isPersistent;//无限时间
    public float Timer { get; private set; }
    public bool IsWorking()
    {
        return (Timer > 0 )||(isPersistent)? true : false;
    }

    public void GainThisBuff(float HoldTime)
    {
        Timer = HoldTime;
    }
    public void GainThisBuff()
    {
        isPersistent = true;
    }
    public void StopThisBuff()
    {
        isPersistent = false;
        Timer = 0;
    }
    public void UpdateTimer()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        if (isPersistent == true)
        {
            if (_IsWorking == false)
            {
                _IsWorking = true;
                onStartWork?.Invoke();
                return;
            }
        }
        else
        {
            if (_IsWorking == false && Timer > 0)
            {
                _IsWorking = true;
                onStartWork?.Invoke();
                return;
            }
            if (_IsWorking == true && Timer <= 0)
            {
                _IsWorking = false;
                onStopWork?.Invoke();
                return;
            }
        }


    }
    public Buff_Scriptable(BuffType type)
    {
        this.type = type;
    }
}
[System.Serializable]
public class Sprite_Scriptable
{
    public SpriteType type;
    public Sprite[] sprites;
}
[System.Serializable]
public class Block_Scriptable
{
    public GameObject prefab;
    public float Strength = 15;
    public int StrengthLevel;
    public ItemType DropItemType;
    public FunctionalBlockType Btype;
    public BlockStrengthType Stype;
    public string breakSounds;
    public Sprite BreakSprite;
}
[System.Serializable]
public class Particles_Scriptable
{
    public ParticleType type;
    public GameObject Particle;
}
[System.Serializable]

public class Bullet_Scriptable
{
    public BulletType type;
    public GameObject bullet;
}

public class ResourceSystem : BaseManager<ResourceSystem>
{
    public Scriptable_Object_List list;
    public Item_Scriptable GetItem(ItemInfo info)
    {
        return list.itemlist[(int)info.type];
    }
    public Item_Scriptable GetItem(ItemType type)
    {
        return list.itemlist[(int)type];
    }
    public Enemy_Scriptable GetEnemy(EnemyType type)
    {
        return list.enemylist[(int)type];
    }
    public Plant_Scriptable GetPlants(PlantsType type)
    {
        return list.plantslist[(int)type];
    }
    public Block_Scriptable GetBlock(FunctionalBlockType type)
    {
        return list.blockList[(int)type];
    }
    public Sprite[] GetSprite(SpriteType type)
    {
        return list.spritelist[(int)type].sprites;
    }
    public GameObject GetParticle(ParticleType type)
    {
        return list.particlelist[(int)type].Particle;
    }
    public GameObject GetBullet(BulletType type)
    {
        return list.bulletlist[(int)type].bullet;
    }


}
