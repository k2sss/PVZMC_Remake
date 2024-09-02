using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerItemManager : BaseManager<PlayerItemManager>
{
    private bool m_isAble = true;
    public bool IsAble {
        set { m_isAble = value; }
        get { return m_isAble; } }
    public ItemType nowType;
    public Transform ItemParent;
    public GameObject[] Items;
    [HideInInspector] public GameObject lastItem;
    [HideInInspector] public GameObject nowItem;
    public float WaveSpeed;
    public Animator animator;
    private AudioSource _audioSource;
    public ParticleSystem EatParticles;
    public GameObject[] SwordLight;
    public GameObject burstBucket;
    [SerializeField] private GameObject lightParticle;
    public FishLine fishLine;
    public GameObject fishMarkPrefab;
    private GameObject fishmark;
    private float EatParticlEmisiionTimer;
    private float EatTimer;
    private GameObject attackThornObject;
    private BulletType arrowType;
    public BlockStrengthType digtype;
    public float digmultiplier = 1;
    public int diglevel = 0;
    private PlayerHealth playerh;
    private bool IsPunch;
    private float ArrowDamageMultiplier = 1;
    [SerializeField]private AudioClip pickClip;
    [SerializeField] private GameObject purifyPowder;
    [SerializeField] private GameObject bloodPowder;
    [SerializeField] private AudioClip heartCrystal;
    private PlantsCatcher cather;
    private PlayerMoveController moveController;

    [SerializeField] private RuntimeAnimatorController normalController;
    [SerializeField] private RuntimeAnimatorController longSwordController;
    private LongSwordLogic longSwordLogic;

    private void Start()
    {
        cather = gameObject.GetComponent<PlantsCatcher>();
        _audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        Items = new GameObject[ItemParent.childCount];
        playerh = gameObject.GetComponent<PlayerHealth>();
        moveController = GetComponent<PlayerMoveController>();
        longSwordLogic = GetComponent<LongSwordLogic>();
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i] = ItemParent.GetChild(i).gameObject;
        }
        nowItem = Items[0];
        lastItem = Items[0];
        EventMgr.Instance.AddEventListener("OnChangeInventory", ChangeType);

    }

    private void Update()
    {
        DoSomeThing();
    }
    public void OnSwitchItem()
    {
        if (fishmark != null)
        {
            fishLine.Disable();
            fishmark.GetComponent<FishMark>().Cancle(false);
            ItemSlotsManager.Instance.SubDurability();
            ObjectPool.Instance.PushObject(fishmark);
            fishmark = null;
        }
    }
    public void ChangeType()
    {
        longSwordLogic.OnQuitLongSword();
        animator.SetBool("Shoot", false);
        animator.SetBool("Eat", false);
        animator.SetBool("Drink", false);
        //���ȡ��
        digtype = BlockStrengthType.normal;
        digmultiplier = 1;
        diglevel = 0;

        PlayerMoveController.Instance.FreeToward = false;
        PlayerMoveController.Instance.IsUseItem = false;
        EatParticles.GetComponent<ParticleSlice>().ChangeSprite(ItemSlotsManager.Instance.GetNowSprite());
        nowType = ItemSlotsManager.Instance.GetnowType();
        if (nowItem != null)
        {
            lastItem = nowItem;
            lastItem.SetActive(false);
        }
        nowItem = Items[(int)ItemSlotsManager.Instance.GetnowType()];
        nowItem.SetActive(true);
        BlockManager.Instance.DisableGizmos();

    }
    public void DoSomeThing()
    {
        if (!m_isAble) return;

        switch (nowType)
        {
            #region ʳ��
            case ItemType.��Ģ��:
                EatFood(2, 2, () => playerh.AddBuff(BuffType.Poison, 10));
                break;
            case ItemType.Ѫ��:
                EatFood(2, 2, () => { });
                break;
            case ItemType.����:
                EatFood(4, 3, () => playerh.AddBuff(BuffType.hungry, 10));
                break;
            case ItemType.ƻ��:
                EatFood(4, 4, () => { });
                break;
            case ItemType.������:
                EatFood(3, 3, () => PlayerHealth.instance.Hurt(1, 0, DamageType.trueDamage));
                break;
            case ItemType.��֭����:
                EatFood(8, 8, () => { });
                break;
            case ItemType.����:
                EatFood(2, 2, () => { });
                break;
            case ItemType.������:
                Drink(1, 14, () =>
                {
                    PlayerHealth.instance.AddBuff(BuffType.Poison, 6);
                    PlayerHealth.instance.AddBuff(BuffType.DickGlowing, 6);
                });
                break;
            case ItemType.��ˮ:
                Drink(0, 0, () => PlayerHealth.instance.AddBuff(BuffType.Chaoes, 45));
                break;
            #endregion
            #region ����
            case ItemType.����̨:
                PutBlock(FunctionalBlockType.CraftTable);
                break;
            case ItemType.����:
                PutBlock(FunctionalBlockType.Iron_Block);
                break;
            case ItemType.ɳʯ��:
                PutBlock(FunctionalBlockType.sand_stone);
                break;
            case ItemType.ʷ��ķ��:
                PutBlock(FunctionalBlockType.slime_block);
                break;
            #endregion
            #region �����򹤾�
            case ItemType.����:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(3, 3, true);
                        SwordSweep(2);
                    }
                    else
                    {
                        Punch(5, 2, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.7f, () => IsPunch = false);
                }
                break;
            case ItemType.����:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(BuffType.DryDamage, 2, 2, 4, true);
                        SwordSweep(5, BuffType.DryDamage, 3, 1, 1);

                    }
                    else
                    {
                        Punch(BuffType.DryDamage, 2, 6, 3, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);
                }
                break;
            case ItemType.TitanSickle:
                if (InputButton() && IsPunch == false)
                {
                    Punch(1, 7);
                    SwordSweep(4);
                    ItemSlotsManager.Instance.SubDurability();
                    GameObject a = Instantiate(SwordLight[3]);
                    a.transform.position = transform.position + new Vector3(0, 0.8f, 0) + transform.forward * 1.3f;
                    a.GetComponent<AttackBox>().InitMove(13, transform.forward, transform.forward);
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.3f, () => IsPunch = false);
                }

                break;
            case ItemType.���������:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(BuffType.DryDamage, 3, 2, 3, true);
                        SwordSweep(4);
                    }
                    else
                    {
                        Punch(BuffType.DryDamage, 3, 6, 2, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);
                }
                break;
            case ItemType.�𻵵�ɳʯ��:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(2, 3, true);
                        SwordSweep(2);
                    }
                    else
                    {
                        Punch(3, 2, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.7f, () => IsPunch = false);
                }
                break;
            case ItemType.ɳʯ��:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(2, 3, true);
                        SwordSweep(2);
                    }
                    else
                    {
                        Punch(5, 2, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.7f, () => IsPunch = false);
                }
                break;
            case ItemType.�綾��:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(2, 3, true);
                        SwordSweep(3, BuffType.Poison, 3);
                    }
                    else
                    {
                        Punch(BuffType.Poison, 3, 5, 2, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.7f, () => IsPunch = false);
                }
                break;
            case ItemType.�綾����:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(1, 3, true);
                        SwordSweep(5, BuffType.Poison, 2, 1.4f);
                    }
                    else
                    {
                        Punch(BuffType.Poison, 3, 5, 2, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.4f, () => IsPunch = false);
                }
                break;
            case ItemType.ŧѪ����:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(1, 3, true);
                        SwordSweep(5, BuffType.Vulnerable, 2, 1.4f);
                    }
                    else
                    {
                        Punch(BuffType.Vulnerable, 3, 5, 2, true);
                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.4f, () => IsPunch = false);
                }
                break;
            case ItemType.̫��:
                AttackButtonUseItem(5, () =>
                {
                    longSwordLogic.OnUseLongSword();
                });
                break;

            case ItemType.ɳʯС��:
                if (InputButton() && IsPunch == false)
                {
                    Punch(3, 2, true);
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.3f, () => IsPunch = false);
                }
                break;
            case ItemType.��:

                if (InputButton() && InventoryManager.Instance.HasArrow() != BulletType.no)
                {
                    ArrowDamageMultiplier = 0.5f;
                    //���ü�
                    arrowType = InventoryManager.Instance.HasArrow();
                    //���ö���
                    animator.SetBool("Shoot", true);
                    //ת��
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 40, 1 << 3))
                    {
                        GameObject a = PlayerHealth.instance.FindTheNearestMob("Enemy", 30);
                        PlayerMoveController.Instance.FaceToward(a.transform.position - transform.position);
                        PlayerMoveController.Instance.FreeToward = true;
                        PlayerMoveController.Instance.IsUseItem = true;
                    }
                    else
                    {
                        PlayerMoveController.Instance.FreeToward = false;
                        PlayerMoveController.Instance.IsUseItem = false;
                    }
                }
                else
                {
                    PlayerMoveController.Instance.FreeToward = false;
                    PlayerMoveController.Instance.IsUseItem = false;
                    animator.SetBool("Shoot", false);
                }
                break;
            case ItemType.���:


                if (InputButton() && IsPunch == false)
                {
                    GameObject a = PlayerHealth.instance.FindTheNearestMob("Enemy", 5);
                    if (a != null)
                    {
                        PlayerMoveController.Instance.FaceToward(a.transform.position - transform.position, 0.4f, 20);
                    }
                    MonoController.Instance.Invoke(0.1f, () => ThrowFishMark(9));
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.2f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);
                    
                }
                break;
            case ItemType.����:
                digtype = BlockStrengthType.wood;
                digmultiplier = 3.4f;
                diglevel = 2;
                if (InputButton() && IsPunch == false)
                {
                    Punch(7, 2, true);
                    IsPunch = true;
                    MonoController.Instance.Invoke(1.5f, () => IsPunch = false);
                }
                break;
            case ItemType.����:
                digtype = BlockStrengthType.stone;
                digmultiplier = 5f;
                diglevel = 2;
                if (InputButton() && IsPunch == false)
                {
                    Punch(3);
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.7f, () => IsPunch = false);
                }
                break;
            case ItemType.����ʹ�߸�:
                digtype = BlockStrengthType.stone;
                digmultiplier = 10f;
                diglevel = 3;
                if (InputButton() && IsPunch == false)
                {
                    Punch(4);
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.7f, () => IsPunch = false);
                }
                break;
            case ItemType.����:
                if (InputButton() && IsPunch == false)
                {
                    if (PlayerMoveController.Instance.GetYSpeed() > -0.1f)
                    {
                        Punch(30, 2, true);
                    }
                    else
                    {
                        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 30, 1 << 3))
                        {
                            Rigidbody rb = PlayerMoveController.Instance._rigidbody;
                            rb.velocity = new Vector3(rb.velocity.x, -20, rb.velocity.z);
                        }
                        PlayerMoveController.Instance.IsFallingDownWithHammer = true;
                        animator.SetBool("Wave", true);
                        MonoController.Instance.Invoke(0.3f, () => animator.SetBool("Wave", false));
                        //���ʱִ��

                    }
                    IsPunch = true;
                    MonoController.Instance.Invoke(1.5f, () => IsPunch = false);
                }
                break;
            case ItemType.Ѫ����:

                if (InputButton() && IsPunch == false)
                {//PC

                    if (attackThornObject == null)
                        attackThornObject = Resources.Load<GameObject>("bullet/AttackThorn/AttackThorn");

                    GameObject go = Instantiate(attackThornObject);
                    go.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
                    GameObject a = PlayerHealth.instance.FindTheNearestMob("Enemy", 5);
                    Vector3 dir = transform.forward + Vector3.up;
                    if (a != null)
                    {
                        dir = (a.transform.position + Vector3.up * ResourceSystem.Instance.GetEnemy(a.GetComponent<Enemy>().type).aimHeight - transform.position).normalized;
                    }
                    go.transform.forward = dir;
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.2f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.3f, () => IsPunch = false);
                    ItemSlotsManager.Instance.SubDurability();
                    PlayerHealth.instance.Hurt(1f, 0, DamageType.trueDamage);
                }

                break;

            case ItemType.����:

                if (InputMgr.GetMouseButtonDown(1) && PhoneControlMgr.PhoneControl == false && IsPunch == false)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 50, 1 << 6))
                    {
                        Grid g = hit.collider.GetComponent<Grid>();

                        if (g.IsEmpty() == false)
                        {
                            g.RemovePlant();
                            SoundSystem.Instance.Play2Dsound("Plant");
                            ItemSlotsManager.Instance.SubDurability();
                        }
                        else if (g.isPlantPotGrid == true)
                        {
                            g.potPlant.targetGrid.RemovePlant();
                            SoundSystem.Instance.Play2Dsound("Plant");
                            ItemSlotsManager.Instance.SubDurability();

                        }



                    }
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.3f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);

                }
                if (PhoneControlMgr.Instance.ClickTwice() && PhoneControlMgr.PhoneControl == true)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 50, 1 << 6))
                    {
                        Grid g = hit.collider.GetComponent<Grid>();
                        if (g.IsEmpty() == false)
                        {
                            g.RemovePlant();
                            SoundSystem.Instance.Play2Dsound("Plant");
                            ItemSlotsManager.Instance.SubDurability();
                        }
                    }
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.3f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);
                }
                break;
            case ItemType.Ѫ������:
                if (InputButton() && IsPunch == false)
                {
                    Punch(9, 2, true);
                    IsPunch = true;
                    MonoController.Instance.Invoke(1.5f, () => IsPunch = false);
                }
                break;
            case ItemType.ͬ־�̽�:

                if (InputButton() && IsPunch == false)
                {
                    Punch(100, 2, true);
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.4f, () => IsPunch = false);
                }
                break;
            case ItemType.����֮��:
                if (InputButton() && IsPunch == false)
                {
                    if(Punch(50, 3, true)!=null)
                    playerh.Hurt(9,0, DamageType.trueDamage);
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.4f, () => IsPunch = false);
                }
                break;



            case ItemType.���칭:
                if (InputButton() && InventoryManager.Instance.HasArrow() != BulletType.no)
                {
                    //���ü�
                    ArrowDamageMultiplier = 0.9f;
                    arrowType = InventoryManager.Instance.HasArrow();
                    //���ö���
                    animator.SetBool("Shoot", true);
                    //ת��
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 40, 1 << 3))
                    {
                        GameObject a = PlayerHealth.instance.FindTheNearestMob("Enemy", 30);
                        PlayerMoveController.Instance.FaceToward(a.transform.position - transform.position);
                        PlayerMoveController.Instance.FreeToward = true;
                        PlayerMoveController.Instance.IsUseItem = true;
                    }
                    else
                    {
                        PlayerMoveController.Instance.FreeToward = false;
                        PlayerMoveController.Instance.IsUseItem = false;
                    }
                }
                else
                {
                    PlayerMoveController.Instance.FreeToward = false;
                    PlayerMoveController.Instance.IsUseItem = false;
                    animator.SetBool("Shoot", false);
                }
                break;
            case ItemType.������:
                if (InputButton() && IsPunch == false)
                {
                    GameObject a = PlayerHealth.instance.FindTheNearestMob("Enemy", 5);
                    if (a != null)
                    {
                        PlayerMoveController.Instance.FaceToward(a.transform.position - transform.position, 0.4f, 20);
                    }
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.2f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);
                    MonoController.Instance.Invoke(0.1f, () => ThrowFishMark(25));
                }
                break;

            #endregion
            #region ����
            case ItemType.ճ�ԹǷ�:
                if (InputMgr.GetMouseButtonDown(1) && PhoneControlMgr.PhoneControl == false && IsPunch == false || (PhoneControlMgr.Instance.ClickTwice() && PhoneControlMgr.PhoneControl == true))
                {//PC
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 50, 1 << 6))
                    {
                        Grid g = hit.collider.GetComponent<Grid>();
                        if (g.IsEmpty() == false)
                        {
                            g.corePlants.AddBuff(BuffType.AttackSpeedUp, 60);
                            SoundSystem.Instance.Play2Dsound("Treat");
                            ItemSlotsManager.Instance.SubCount();
                        }
                    }
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.3f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);

               
                } break;
            case ItemType.Ӳ���Ƿ�:
                if (InputMgr.GetMouseButtonDown(1) && PhoneControlMgr.PhoneControl == false && IsPunch == false || (PhoneControlMgr.Instance.ClickTwice() && PhoneControlMgr.PhoneControl == true))
                {//PC
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 50, 1 << 6))
                    {
                        Grid g = hit.collider.GetComponent<Grid>();
                        if (g.IsEmpty() == false)
                        {
                            g.corePlants.AddBuff(BuffType.solid, 60);
                            SoundSystem.Instance.Play2Dsound("Treat");
                            ItemSlotsManager.Instance.SubCount();
                        }
                    }
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.3f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);
                }
                break;
            case ItemType.��������:
                if (InputMgr.GetMouseButtonDown(1) && PhoneControlMgr.PhoneControl == false && IsPunch == false || (PhoneControlMgr.Instance.ClickTwice() && PhoneControlMgr.PhoneControl == true))
                {
                    if (PlantManager.Instance.IsSelectedPlant()) return;


                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 50, 1 << 6))
                    {
                        Grid g = hit.collider.GetComponent<Grid>();

                        if (g.IsEmpty() == false && !cather.isCatched)
                        {
                            cather.CatchPlant(g.corePlants);
                            SoundSystem.Instance.Play2Dsound("Plant");
                            ItemSlotsManager.Instance.SubDurability();
                        }
                        else if(g.IsEmpty()&& cather.isCatched&&g.IsFitForPlant(cather.GetCatchePlantType()))
                        {
                            cather.PutPlant(g);
                            
                            SoundSystem.Instance.Play2Dsound("Plant");
                            ItemSlotsManager.Instance.SubDurability();
                        }
                       
                    }
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.3f, () => animator.SetBool("Wave", false));
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.5f, () => IsPunch = false);
                }
                    break;
            case ItemType.Ѫ��:
                AttackButtonUseItem(2f, () =>
                {
                    if (!MySystem.IsInLevel() || WeatherManager.Instance.IsBloodMoon)
                        return;
                    //���ĵƹ�
                    WeatherManager.Instance.TurnToBloodMoon();
                    //��Ч
                    SoundSystem.Instance.Play2Dsound("BossRoar");

                    EnemyManager.Instance.k *= 1.4f;

                    ItemSlotsManager.Instance.SubCount();

                    MusicMgr.Instance.PlayMusic("bloodMoon");

                });
                break;
            case ItemType.��Ͱ:

                AttackButtonUseItem(1f, () =>
                {
                    GameObject b = Instantiate(burstBucket);
                    b.transform.position = transform.position;
                    ItemSlotsManager.Instance.SubCount();
                    SoundSystem.Instance.Play2Dsound("Wood");
                    WaveHand();
                });
                break;
            case ItemType.����ˮ��:
                AttackButtonUseItem(1f, () =>
                {
                    if (playerh.MaxHealth >= 40f)
                        return;
                    
                    playerh.ChangeMaxHealth(playerh.MaxHealth + 10);
                    ItemSlotsManager.Instance.SubCount();
                    SoundSystem.Instance.Play2Dsound(heartCrystal);
                    WaveHand();
                });
                break;

           
            case ItemType.���:

                AttackButtonUseItem(1f, () =>
                {
                    SoundSystem.Instance.PlayRandom2Dsound("Ignite");
                    GameObject b = Instantiate(lightParticle);
                    b.transform.position = transform.position + Vector3.up;
                    ItemSlotsManager.Instance.SubCount();
                    WaveHand();
                });
                break;
            case ItemType.��Я����:
                AttackButtonUseItem(0.6f, () =>
                                 {
                                     if (CardSlot.Instance != null)
                                     {

                                         SoundSystem.Instance.Play2Dsound(pickClip);
                                         CardSlot.Instance.AddSunCont(25);
                                         CardSlot.Instance.AllCardsSetConsumeMask();
                                         ItemSlotsManager.Instance.SubCount();
                                         WaveHand();

                                     }
                                 });
                break;
            case ItemType.������ĩ:
                AttackButtonUseItem(1f, () =>
                {
                    GameObject g = Instantiate(purifyPowder);
                    g.transform.position = transform.position;
                    ItemSlotsManager.Instance.SubCount();
                    WaveHand();
                });
                break;

            case ItemType.�ɺ��ĩ:
                AttackButtonUseItem(1f, () =>
                {
                    GameObject g = Instantiate(bloodPowder);
                    g.transform.position = transform.position;
                    ItemSlotsManager.Instance.SubCount();
                    WaveHand();
                });
                break;
            case ItemType.�ɺ�ҩˮ:
                Drink(0, 0, () => PlayerHealth.instance.AddBuff(BuffType.VampireAbility, 30));
                break;
            case ItemType.����ҩˮ:
                Drink(0, 0, () => PlayerHealth.instance.AddBuff(BuffType.Satiety, 60));
                break;
            case ItemType.����ҩˮ:
                Drink(0, 0, () => PlayerHealth.instance.AddBuff(BuffType.ProbabilisticDamage, 60));
                break;
            #endregion
            default:
                if (InputButton() && IsPunch == false)
                {
                    Punch(1);
                    IsPunch = true;
                    MonoController.Instance.Invoke(0.7f, () => IsPunch = false);

                }

                break;

        }
    }
    private bool InputButton()
    {
        if (PhoneControlMgr.Instance != null && PhoneControlMgr.PhoneControl == true)
        {
            return PhoneControlMgr.Instance.IsAttackButtonPressed();
        }
        else
        {
            return InputMgr.GetMouseButton(1);
        }

    }
    private bool InputButtonDown()
    {
        if (PhoneControlMgr.Instance != null && PhoneControlMgr.PhoneControl == true)
        {
            return PhoneControlMgr.Instance.IsAttackButtonDown();
        }
        else
        {
            return InputMgr.GetMouseButtonDown(1);
        }
    }


    public Mob Punch(float damage = 1, float Range = 2, bool SubDurability = false)
    {
        return Punch(BuffType.nobuff, 0, damage, Range, SubDurability);
    }
    public Mob Punch(BuffType bufftype, float BuffTime, float damage = 1, float Range = 2, bool SubDurability = false)
    {
        animator.SetBool("Wave", true);
        MonoController.Instance.Invoke(0.2f, () => animator.SetBool("Wave", false));
        GameObject targetGameObject = PlayerHealth.instance.FindTheNearestMob("Enemy", Range);
        Mob targetMob = null;
        if (targetGameObject != null)
        {
            targetMob = targetGameObject.GetComponent<Mob>();
            PlayerMoveController.Instance.FaceToward(targetGameObject.transform.position - transform.position, 0.4f, 10);
            if (PlayerMoveController.Instance.GetYSpeed() < -0.1f)//�ػ�
            {
                SoundSystem.Instance.PlayRandom2Dsound("StrongPunch");
                AttackMgr.AttackWithBuff(playerh, targetMob, 1.5f * damage * playerh.FinalDamage, bufftype, BuffTime);
                //������Ч
                GameObject particle_HIT = Instantiate(ResourceSystem.Instance.GetParticle(ParticleType.critical_hit));
                particle_HIT.transform.position = targetGameObject.transform.position + new Vector3(0, 1, 0);
            }
            else
            {
                SoundSystem.Instance.PlayRandom2Dsound("WeakPunch");
                AttackMgr.AttackWithBuff(playerh, targetMob, damage * playerh.FinalDamage, bufftype, BuffTime);

            }
            if (SubDurability) ItemSlotsManager.Instance.SubDurability();
            PlayerHunger.Instance.AddHunger(0.1f);
        }
        ///������Ѫ
        if (BaseLevelEvent.Instance != null && BaseLevelEvent.Instance.PlayerAttackHurt == true)
        {
            PlayerHealth.instance.Hurt(4, 0, DamageType.trueDamage);
        }
        return targetMob;


    }
    #region eatSomething
    public void EatFood(int hunger, int hunger_overflow, Action action)
    {
        if (PlayerHunger.Instance.Hunger < PlayerHunger.Instance.MaxHunger)
        {
            if (InputButton())
            {
                PlayerMoveController.Instance.IsUseItem = true;
                animator.SetBool("Eat", true);
                EatFoodEmission();
                EatTimer += Time.deltaTime;
                if (EatTimer > 1f)
                {
                    EatTimer = 0;

                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHunger>().EatFood(hunger, hunger_overflow);
                    ItemSlotsManager.Instance.SubCount();
                    HungerManager.instance.AllInfoUpdate();
                    action();
                }
            }
            else
            {
                EatTimer = 0;
                animator.SetBool("Eat", false);
                PlayerMoveController.Instance.IsUseItem = false;
            }
        }

    }
    public void Drink(int x, int y, Action action)
    {
        if (InputButton())
        {
            PlayerMoveController.Instance.IsUseItem = true;
            animator.SetBool("Drink", true);
            EatFoodEmission();
            EatTimer += Time.deltaTime;
            if (EatTimer > 1f)
            {
                EatTimer = 0;

                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHunger>().EatFood(x, y);
                ItemSlotsManager.Instance.SubCount();
                HungerManager.instance.AllInfoUpdate();
                action();
            }
        }
        else
        {
            EatTimer = 0;
            animator.SetBool("Drink", false);
            PlayerMoveController.Instance.IsUseItem = false;
        }
    }
    private void EatFoodEmission()
    {
        EatParticlEmisiionTimer += Time.deltaTime;
        if (EatParticlEmisiionTimer > 0.1f)
        {
            EatParticlEmisiionTimer = 0;
            EatParticles.Emit(1);
        }
    }

    private void EatSound()
    {
        AudioClip[] aclips = SoundSystem.Instance.GetAudioClips("Eat");
        _audioSource.PlayOneShot(aclips[UnityEngine.Random.Range(0, aclips.Length)]);
    }
    private void DrinkSound()
    {
        AudioClip[] aclips = SoundSystem.Instance.GetAudioClips("Drink");
        _audioSource.PlayOneShot(aclips[UnityEngine.Random.Range(0, aclips.Length)]);
    }
    private void EatFullSound()
    {
        if (PlayerHunger.Instance.Hunger == PlayerHunger.Instance.MaxHunger)
        {

            AudioClip[] aclips = SoundSystem.Instance.GetAudioClips("EatFull");
            _audioSource.PlayOneShot(aclips[0]);
        }

    }
    #endregion
    public void PutBlock(FunctionalBlockType type)
    {
        if (PlantManager.Instance.IsSelectedPlant() == false && BlockManager.Instance.WithInRange(BlockManager.Instance.GetBlockPos()) && !GridManager.Instance.ThisPlaceIsOccpuied())
        {
            BlockManager.Instance.EnableGizmos();
            if (InputMgr.GetMouseButtonDown(1) && PhoneControlMgr.PhoneControl == false
                || (PhoneControlMgr.PhoneControl == true && PhoneControlMgr.Instance.ClickTwice()))
            {
                FunctionalBlock f = BlockManager.Instance.PutABlock(type, BlockManager.Instance.GetBlockPos());
                if (f != null)
                {
                    animator.SetBool("Wave", true);
                    MonoController.Instance.Invoke(0.1f, () => animator.SetBool("Wave", false));
                    ItemSlotsManager.Instance.SubCount();
                    f.PlayPutDownSound();
                    PlayerMoveController.Instance.FaceToward(f.transform.position - transform.position, 0.2f, 10);
                }
            }
        }
        else
        {
            BlockManager.Instance.DisableGizmos();
        }

    }
    public void SwordSweep(float damage)
    {
        SwordSweep(damage, BuffType.nobuff, 0, 1);
    }
    public void AttackButtonUseItem(float timeInterval, Action action)
    {
        if (InputButton() && IsPunch == false)
        {
            action?.Invoke();
            IsPunch = true;
            MonoController.Instance.Invoke(timeInterval, () => IsPunch = false);
        }

    }

    public void SwordSweep(float damage, BuffType bufftype, float timer, float size = 1, int swordLightType = 0)
    {
        GameObject s = Instantiate(SwordLight[swordLightType]);
        s.transform.localScale *= size;
        s.transform.position = transform.position + new Vector3(0, 0.8f, 0) + transform.forward * 1.3f;
        s.transform.right = transform.right;
        s.GetComponent<SwordObj>().Init(damage * PlayerHealth.instance.FinalDamage, bufftype, timer, playerh);
        PlayerHealth.instance.PlayRandomSounds(SoundSystem.Instance.GetAudioClips("Wave"), 0.7f);
    }
    public void ThrowStuff(ItemInfo info)
    {
        GameObject t = Instantiate(ResourceSystem.Instance.GetItem(info).prefab);
        //Debug.Log(info.Durability);
        t.transform.position = transform.position + new Vector3(0, 1, 0);

        Item i = t.GetComponent<Item>();
        i.info = info.ShallowClone();
        i.info.Durability = info.Durability;

        i.OnThrow();
    }
    public void DoNothing()
    {


    }
    public void WaveHand(float Interval = 0.2f)
    {
        animator.SetBool("Wave", true);
        MonoController.Instance.Invoke(Interval, () => animator.SetBool("Wave", false));
    }
    public void Shoot()
    {
        int pos = 0;
        switch (arrowType)
        {
            case BulletType.Normal_Arrow:
                InventoryManager.Instance.ThereIsItem(ItemType.��, ref pos);
                InventoryManager.Instance.SubTargetSlotItem(pos);
                ItemSlotsManager.Instance.SubDurability();
                ShootArrow(BulletType.Normal_Arrow, 20 * ArrowDamageMultiplier);
                break;
            case BulletType.Sand_Arrow:

                InventoryManager.Instance.ThereIsItem(ItemType.��_ɳʯ, ref pos);
                InventoryManager.Instance.SubTargetSlotItem(pos);
                ItemSlotsManager.Instance.SubDurability();
                ShootArrow(BulletType.Sand_Arrow, 10 * ArrowDamageMultiplier);
                break;
        }
    }
    private void ShootArrow(BulletType type, float damage = 10, float speed = 20)
    {
        _audioSource.PlayOneShot(SoundSystem.Instance.GetAudioClips("Bow")[0]);
        GameObject arrowObj = ObjectPool.Instance.GetObject(ResourceSystem.Instance.GetBullet(type));
        arrowObj.transform.position = transform.position + new Vector3(0, 1, 0);
        Bullet bullet = arrowObj.GetComponent<Bullet>();
        bullet.transform.right = transform.right;
        bullet.init(transform.forward, damage, speed);

    }
    private void ThrowFishMark(int fishingPower = 15)
    {

        if (fishLine.IsAble == false)
        {
            fishLine.Enable();
            //�׳�
            SoundSystem.Instance.Play2Dsound("Bow", 0.7f);
            fishmark = ObjectPool.Instance.GetObject(fishMarkPrefab);
            fishmark.transform.position = fishLine.Starttransform.position;
            fishLine.endtransform = fishmark.transform;
            fishmark.GetComponent<Rigidbody>().velocity = transform.forward * 6 + new Vector3(0, 2, 0);
        }
        else
        {
            ItemType t = ItemSlotsManager.Instance.GetnowType();
            if (t== ItemType.���||t == ItemType.������)
                ItemSlotsManager.Instance.SubDurability();
            fishLine.Disable();
            FishMark mark = fishmark.GetComponent<FishMark>();
            mark.Cancle(true);
            mark.SetFishingPower(fishingPower);
            ObjectPool.Instance.PushObject(fishmark);
            fishmark = null;
        }

    }
    public void HitGround()
    {

        GameObject a = ObjectPool.Instance.GetObject(ResourceSystem.Instance.GetParticle(ParticleType.HeavyHit));
        a.transform.position = transform.position;
        CameraAction.Instance.StartShake();

        _audioSource.PlayOneShot(SoundSystem.Instance.GetAudioClips("Shock")[0]);

        ItemSlotsManager.Instance.SubDurability();
        ItemSlotsManager.Instance.SubDurability();

        //����
        PlayerHunger.Instance.AddHunger(2);

        GameObject[] enemiesInRange = playerh.FindTheNearestMobs("Enemy", 3);

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            Enemy enemy = enemiesInRange[i].GetComponent<Enemy>();
            enemy.Hurt(20);
            enemy.Vertigo(0.4f);
            Vector3 dir = (enemy.transform.position - transform.position).normalized * 7;
            enemy.AddForce(dir);
        }

    }
    public void TurnToNormalAnimatior()
    {
        animator.runtimeAnimatorController = normalController;
    }
    public void TurnToLongSwordAnimator()
    {
        animator.runtimeAnimatorController = longSwordController;
    }
}
