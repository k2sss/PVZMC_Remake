using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Plants : Mob
{
    public PlantsType type;
    public bool Disable;
    [HideInInspector] public Grid targetGrid { get; set; }
    [HideInInspector] public float MaybeUseTimer;//����ֲ����ܻ��õ���TIMER�����ڸ�����Ϊ�˴浵
    [HideInInspector] public float MaybeUseTimer2;
    [HideInInspector] public bool MaybeUseBool;//���ڸ�����Ϊ�˴浵
    [HideInInspector] public bool MaybeUseBool2;
    [SerializeField]private bool isCatchable = true;
    private void CheckGridState()
    {
        Ray ray = new Ray(transform.position + Vector3.down * 1.5f, Vector3.up);
        //Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 1f, Color.blue, 1);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, 1 << 6))
        {
            Grid g = hit.collider.GetComponent<Grid>();
            if (g != null)
            {
                targetGrid = g;
            }
        }
        if (targetGrid == null)
        {
            return;
        }
        if (targetGrid.gridType == GridType.blood)
        {
            AddBuff(BuffType.CrimsonDecay, 1);
        }
        else
        {
            StopBuff(BuffType.CrimsonDecay);
        }

    }
    public bool IsCatchable()
    {
        return isCatchable;
    }
    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = 9;
        gameObject.tag = "Plants";
        _collider.isTrigger = true;
    }
    protected override void Start()
    {
        base.Start();
        
        EventMgr.Instance.AddEventListener("GameWin", () => Disable = true);
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        InvokeRepeating("CheckGridState", 0.1f, 1);

    }
    protected override void Update()
    {
        base.Update();
   
        
    }
    public override void Death()
    {
        base.Death();

        if (targetGrid != null)
        {
            targetGrid.RemovePlantBind();
        }
        Destroy(gameObject);
    }
}
