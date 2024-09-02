using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : FunctionalBlock
{
    public Transform rootPos;//根部
    public Transform endPos;//头部
    private float timer;
    private float RandomTime;
    public Direction dir;
    private GameObject thorn;
    private GameObject thorn2;
    private float CircleTimer;
    public float CircleTime;
    public bool IsPure = false;
    protected override void Awake()
    {
        blockrenderer = GetComponent<Renderer>();
        gameObject.tag = "Blocks";
        _collider = GetComponent<Collider>();

    }
    public void TurnPure()
    {
        IsPure = true;
        Btype = FunctionalBlockType.Thorn2;
    }
    public void TurnBlood()
    {
        IsPure = false;
        Btype = FunctionalBlockType.Thorn;
    }
    protected override void Start()
    {
        base.Start();
        Init();
    }
    public void Init()
    {
        thorn = FileLoadSystem.ResourcesLoad<GameObject>("prefab/Thorn");
        thorn2 = FileLoadSystem.ResourcesLoad<GameObject>("prefab/Thorn2");
        RandomTime = Random.Range(5, 10);
    }
    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (timer > RandomTime)
        {
            RandomTime = Random.Range(5, 10);
            timer = 0;
            Reproduce();
        }
        CircleTimer += Time.deltaTime;
        if (CircleTimer > CircleTime && CircleTimer < CircleTime + 0.1f)
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;

            if (CircleTimer > CircleTime + 0.1f)
            {
                CircleTimer = 0;
            }
        }

    }
    private Vector3 DirTrans(Direction dir)
    {
        if (dir == Direction.up) return Vector3.up;
        if (dir == Direction.down) return Vector3.down;
        if (dir == Direction.left) return Vector3.left;
        if (dir == Direction.right) return Vector3.right;
        if (dir == Direction.forward) return Vector3.forward;
        if (dir == Direction.behind) return Vector3.back;
        return Vector3.up;
    }
    private bool Check(Direction dir)
    {
        if (dir == Direction.up && this.dir == Direction.down || dir == Direction.down && this.dir == Direction.up
          || dir == Direction.left && this.dir == Direction.right || dir == Direction.right && this.dir == Direction.left
          || dir == Direction.forward && this.dir == Direction.behind || dir == Direction.behind && this.dir == Direction.right)
        {
            return false;
        }
        Ray ray = new Ray(transform.position + 0.5f * DirTrans(this.dir), DirTrans(dir));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.6f, (1 << 17) | (1 << 3)))
        {
            return false;
        }
        return true;
    }
    public void Reproduce()//生长
    {
        List<Direction> dirs = new List<Direction>();
        for (int i = 0; i < 6; i++)
        {
            if (Check((Direction)i))
            {
                dirs.Add((Direction)i);
            }
        }
        if (dirs.Count > 0)
        {
            Direction targetDir = dirs[Random.Range(0, dirs.Count)];
            Create(targetDir);
        }
    }
    private void Create(Direction dir)
    {
        if (info_int_1 > 0)
        {
            GameObject g;
            if (IsPure == false)
            {

               g = Instantiate(thorn);
            }
            else
            {
               g = Instantiate(thorn2);
            }
            g.transform.position = endPos.position;
            if (dir == Direction.down)
            {
                g.transform.Rotate(0, 0, 180);
            }
            else if (dir == Direction.left)
            {
                g.transform.Rotate(0, -90, 0);
            }
            else if (dir == Direction.right)
            {
                g.transform.Rotate(0, 90, 0);
            }
            else if (dir == Direction.forward)
            {
                g.transform.Rotate(-90, 0, 0);
            }
            else if (dir == Direction.behind)
            {
                g.transform.Rotate(90, 0, 0);
            }
            Thorn t = g.GetComponent<Thorn>();
            t.info_int_1 = info_int_1 - 1;
            t.dir = dir;
            info_int_1--;
        }



    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsPure == false)
        {
            if (other.CompareTag("Player"))
            {
                if (PlayerHealth.instance.isInvincibletimer < 0)
                {
                    PlayerHealth.instance.Hurt(2, 0, DamageType.trueDamage);
                    Break();
                }
            }
            else if (other.CompareTag("Plants"))
            {
                other.GetComponent<Plants>().Hurt(4);
                Break();
            }
        }
        else
        {

            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().Hurt(3);
                Break();
            }
        }

    }
    public enum Direction
    {
        up,
        down,
        forward,
        behind,
        left,
        right,

    }
}
