using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalBlock : MonoBehaviour
{
    public BlockSaveInfo info;
    protected Renderer blockrenderer;
    public Vector4 HighLightColor = new Vector4(2, 2, 2, 1);
    public FunctionalBlockType Btype;
    public float health = 15;
    [HideInInspector] public float MaxHealth = 15;
    private SpriteRenderer[] breakSheetrenderes;
    public int info_int_1;
    
    protected Collider _collider;
    private bool OnPut;
    protected bool IsEnter;
    public bool CanDig;
    protected virtual void Awake()
    {
        blockrenderer = GetComponent<Renderer>();
        gameObject.layer = 3;
        gameObject.tag = "Blocks";
        _collider = GetComponent<Collider>();
        _collider.sharedMaterial = Resources.Load<PhysicMaterial>("Smooth");

    }
    protected virtual void Start()
    {
        InitBreakSheet();
        ChangeBreakSheet(1);
        health = ResourceSystem.Instance.GetBlock(Btype).Strength;
        MaxHealth = health;
       
        MonoController.Instance.Invoke(0.1f, () => OnPut = true);
        MonoController.Instance.Invoke(0.2f, () => CanDig = true);
    }
    protected virtual void Update()
    {

    }
    //causeObject 为 0 代表攻击方为敌对方或者植物方，为1代表为史蒂夫
    public void CauseDamage(float BaseDamage, BlockStrengthType type, int StrengthLevel, float Multiplier = 1, float DropPercentage = 1, int causeObject = 0)
    {
        if (type == ResourceSystem.Instance.GetBlock(Btype).Stype)
        {
            health -= (BaseDamage * Multiplier);
        }
        else
        {
            health -= BaseDamage;
        }

        //Debug.Log("start");
        ChangeBreakSheet(health / MaxHealth);
        if (health <= 0)
        {  
            if (Random.Range(0, 1f) <= DropPercentage && ResourceSystem.Instance.GetBlock(Btype).StrengthLevel <= StrengthLevel && (type == BlockStrengthType.normal || type == ResourceSystem.Instance.GetBlock(Btype).Stype))
            {
               
                DropItem();
               
            }
            
            if (causeObject == 1 && ItemSlotsManager.Instance.GetMaxDurability() != 1)
            {
                ItemSlotsManager.Instance.SubDurability();
            }

            Break();
        }
    }
    //public void OnMouseEnter()
    //{
    //    IsEnter = true;
    //    BlockManager.Instance.target = this;
    //}
    //public void OnMouseExit()
    //{
    //    IsEnter = false;
    //    BlockManager.Instance.target = null;
    //}

    public void PlayPutDownSound()
    {
        SoundSystem.Instance.PlayRandom2Dsound(SoundSystem.Instance.GetAudioClips(ResourceSystem.Instance.GetBlock(Btype).breakSounds));
    }

    public void Break()
    {

        if (SoundSystem.Instance.GetAudioClips(ResourceSystem.Instance.GetBlock(Btype).breakSounds).Length != 0)
            SoundSystem.Instance.PlayRandom2Dsound(SoundSystem.Instance.GetAudioClips(ResourceSystem.Instance.GetBlock(Btype).breakSounds));
        //生成粒子
        GameObject breakParticle = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.break_particles));
        breakParticle.transform.position = transform.position + new Vector3(0, 0.8f, 0);
        ParticleSlice pc = breakParticle.GetComponent<ParticleSlice>();
        if (ResourceSystem.Instance.GetBlock(Btype).BreakSprite != null)
        {
            pc.CutedSprite = ResourceSystem.Instance.GetBlock(Btype).BreakSprite;
            pc.GetComponent<ParticleSystem>().Emit(15);
        }
        BlockManager.Instance.Remove(info, this.gameObject);

    }

    public GameObject DropItem()
    {
        GameObject go = null;
        
        if (ResourceSystem.Instance.GetBlock(Btype).DropItemType != ItemType.Nothing)
        {
           
            go = Instantiate(ResourceSystem.Instance.GetItem(ResourceSystem.Instance.GetBlock(Btype).DropItemType).prefab);
            go.transform.position = transform.position + new Vector3(0, 1, 0);

        }
        return go;
    }
    private void InitBreakSheet()
    {
        GameObject breakSheet = FileLoadSystem.ResourcesLoad<GameObject>("break-sheet");
        breakSheetrenderes = new SpriteRenderer[6];
        for (int i = 0; i < 6; i++)
        {
            breakSheetrenderes[i] = Instantiate(breakSheet, transform).GetComponent<SpriteRenderer>();
            breakSheetrenderes[i].transform.localScale /= 100;
        }
        breakSheetrenderes[0].transform.position = transform.position + new Vector3(0, 1.01f, 0);
        breakSheetrenderes[1].transform.position = transform.position + new Vector3(0, -0.01f, 0);
        breakSheetrenderes[1].transform.Rotate(180, 0, 0);
        breakSheetrenderes[2].transform.position = transform.position + new Vector3(0.501f, 0.5f, 0);
        breakSheetrenderes[2].transform.Rotate(0, 90, 0);
        breakSheetrenderes[3].transform.position = transform.position + new Vector3(-0.501f, 0.5f, 0);
        breakSheetrenderes[3].transform.Rotate(0, 90, 0);
        breakSheetrenderes[4].transform.position = transform.position + new Vector3(0, 0.5f, 0.501f);
        breakSheetrenderes[4].transform.Rotate(90, 0, 0);
        breakSheetrenderes[5].transform.position = transform.position + new Vector3(0, 0.5f, -0.501f);
        breakSheetrenderes[5].transform.Rotate(90, 0, 0);

    }
    public void ChangeBreakSheet(float num)
    {
        if (num < 0)
        {
            num = 0;
        }
        int len = ResourceSystem.Instance.GetSprite(SpriteType.breakSheet).Length - 1;
        int m = len - (int)(len * num);
        if (num < 0.9f)
        {
            for (int i = 0; i < breakSheetrenderes.Length; i++)
            {
                breakSheetrenderes[i].sprite = ResourceSystem.Instance.GetSprite(SpriteType.breakSheet)[m];
            }
        }
        else
        {
            for (int i = 0; i < breakSheetrenderes.Length; i++)
            {
                breakSheetrenderes[i].sprite = ResourceSystem.Instance.GetSprite(SpriteType.breakSheet)[0];
            }
        }


    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && OnPut == false)
        {
            collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, transform.position.y + 1, collision.gameObject.transform.position.z);
            OnPut = true;
        }

    }
}

