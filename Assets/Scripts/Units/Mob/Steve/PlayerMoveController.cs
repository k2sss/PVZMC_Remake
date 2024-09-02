using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : BaseManager<PlayerMoveController>
{
    public Rigidbody _rigidbody;
    private Animator _animator;
    public Mob PlayerMob;
    public float GravityForce = 4;
    public float JumpForce = 4;
    public float SwimForce = 1f;
    private Vector3 InputDir;
    public bool _CanJump { get; private set; }

    public bool IsSwimming;
    public bool IsReverse { get; set; }
    public bool IsBreakingBlock { set; get; }
    public bool IsUseItem { set; get; }
    public bool IsHungry { set; get; }
    public bool FreeToward { set; get; }
    public Vector3 MoveDir { private set; get; }
    private float TurnSpeed = 5;
    private float DefaultTurnSpeed = 5;
    public bool IsFallingDownWithHammer;
    public bool SandStormEffected;
    public float WindStrength;
    private float jumpTimer;
    public float Yspeed;
    private bool isAble = true;
    private void CheckIsHugry()
    {
        if (PlayerHunger.Instance.Hunger <= 6)
        {
            IsHungry = true;
        }
        else
        {
            IsHungry = false;
        }

    }
    public bool IsMove()
    {
        if (InputDir != Vector3.zero)
        {
            return true;
        }
        return false;
    }
    public bool IsJump()
    {
        if (_CanJump == false)
            return true;
        else
            return false;
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        PlayerHunger.Instance.onHungerChange += CheckIsHugry;
        MonoController.Instance.Invoke(0.1f, () => _rigidbody.interpolation = RigidbodyInterpolation.Interpolate);
    }

    private void Update()
    {


        if (PhoneControlMgr.PhoneControl == true)
        {
            Vector2 a = PhoneControlMgr.Instance.handler.OutPut;
            InputDir = new Vector3(a.x, 0, a.y);
        }
        else
        {
            InputDir = new Vector3(InputMgr.GetAxisRaw("Horizontal"), 0, InputMgr.GetAxisRaw("Vertical"));
        }
        Yspeed = _rigidbody.velocity.y;
        CheckGround();


    }
    private void FixedUpdate()
    {
        if (!isAble) return;

        if (IsBreakingBlock == false && IsUseItem == false && IsHungry == false)
            Move(InputDir, PlayerMob.FinalSpeed);
        else
            Move(InputDir, PlayerMob.FinalSpeed / 2);
        Face(MoveDir, TurnSpeed);

        Jump();
    }
    private void Jump()//跳跃
    {
        if (IsSwimming == false)
        {
            jumpTimer += Time.deltaTime;
            if (_CanJump == true && jumpTimer > 0.2f)
            {
                if (PhoneControlMgr.Instance != null && PhoneControlMgr.PhoneControl == true)
                {
                    if (PhoneControlMgr.Instance.IsJumpButtonPressed())
                    {
                        jumpTimer = 0;
                        PlayerHunger.Instance.AddHunger(0.1f);
                        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y + JumpForce, _rigidbody.velocity.z);
                    }
                }
                else
                if (InputMgr.GetKey(KeyCode.Space))
                {
                    jumpTimer = 0;
                    PlayerHunger.Instance.AddHunger(0.1f);
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y + JumpForce, _rigidbody.velocity.z);
                }

            }
        }
        else
        {
            if (PhoneControlMgr.Instance != null && PhoneControlMgr.PhoneControl == true)
            {
                if (PhoneControlMgr.Instance.IsJumpButtonPressed())
                {
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y + SwimForce * Time.fixedDeltaTime, _rigidbody.velocity.z);
                }
            }
            else
               if (InputMgr.GetKey(KeyCode.Space))
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y + SwimForce * Time.fixedDeltaTime, _rigidbody.velocity.z);
            }
        }


    }

    public void Move(Vector3 MoveDirection, float Speed)//刚体移动
    {

        Vector3 Dir = MoveDirection.normalized * Speed;
        if (IsReverse)
            Dir = -Dir;
        if (SandStormEffected == false)
            _rigidbody.velocity = new Vector3(Dir.x, _rigidbody.velocity.y - GravityForce, Dir.z) + PlayerHealth.instance.WaterForce;
        else
            _rigidbody.velocity = new Vector3(Dir.x - WindStrength, _rigidbody.velocity.y - GravityForce, Dir.z) + PlayerHealth.instance.WaterForce;
        if (Dir != Vector3.zero)
        {
            if (FreeToward == false)
                MoveDir = Dir;
            _animator.SetBool("Walk", true);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }
    }
    public void Face(Vector3 Dir, float speed)
    {
        Vector3 toward = new Vector3(Dir.x, transform.forward.y, Dir.z);
        transform.forward = Vector3.Slerp(transform.forward, toward, speed * Time.deltaTime);
    }
    private void CheckGround()//检测是否落地，约束跳跃为1次
    {
        float Height = 0.2f;
        Ray ray = new Ray(transform.position + new Vector3(0, 0.1f, 0), Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Height, 1 << 3))
        {
            if (_CanJump == false && IsFallingDownWithHammer == true)
            {
                PlayerItemManager.Instance.HitGround();
                IsFallingDownWithHammer = false;
            }
            _CanJump = true;
        }
        else
        {
            _CanJump = false;
        }
    }

    public void FaceToward(Vector3 toward)//面朝
    {
        MoveDir = new Vector3(toward.x, 0, toward.z);
    }
    public void FaceToward(Vector3 toward, float CancleTime, float TurnSpeed = 5)
    {
        MoveDir = new Vector3(toward.x, 0, toward.z);
        FreeToward = true;
        MonoController.Instance.Invoke(CancleTime, () => FreeToward = false);
        this.TurnSpeed = TurnSpeed;
        MonoController.Instance.Invoke(CancleTime, () => this.TurnSpeed = DefaultTurnSpeed);
    }
    public float GetYSpeed()
    {
        return _rigidbody.velocity.y;
    }

    public void DisAleAllControl()
    {
        isAble = false;
    }
    public void EnableAllControl()
    {
        isAble = true;
    }
    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }


    public void OnAnimatorMove()
    {
        Vector3 horizontal = new Vector3(_animator.deltaPosition.x, 0, _animator.deltaPosition.z);
        transform.position += horizontal.magnitude * transform.forward + Vector3.up * _animator.deltaPosition.y;
    }


}
