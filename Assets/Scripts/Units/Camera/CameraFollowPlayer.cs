using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : BaseManager<CameraAction>
{
    public bool isStatic;
    public Transform targetPosWhenStatic;
    private bool IsFollowPlayer = true;
    private Transform _PlayerTransform;
    public float FollowSpeed;
    public Vector3 FollowPosOffSet;
    public Transform LeftLimit;
    public Transform RightLimit;
    public Transform UpLimit;
    public Transform DownLimit;
    public bool IsShake { private set; get; }
    private float ShakeStrength;
    private float ShakeSpeed;
    private float GoToFixedPosSpeed;
    private Vector3 targetPos;
    private Vector3 originPos;
    private Vector3 defaulteulerAngleTarget;
    public Vector3 eulerAngleTarget;
    public Vector3 CameraOffset = new Vector3(12, 0, 0);
    protected override void Awake()
    {
        base.Awake();
        if(isStatic == false)
        _PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        else
        {
        _PlayerTransform = targetPosWhenStatic;
        }
        transform.position = _PlayerTransform.position + FollowPosOffSet;
       
        defaulteulerAngleTarget = transform.eulerAngles;
    }
  
    void Update()
    {

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, eulerAngleTarget, 10 * Time.unscaledDeltaTime);

        // transform.position = Vector3.Lerp(transform.position, _PlayerTransform.position + FollowPosOffSet, FollowSpeed * Time.fixedDeltaTime);
        //跟随玩家
        if (IsFollowPlayer == true)
        {
            Vector3 playerPosAfterClamp = new Vector3(Mathf.Clamp(_PlayerTransform.position.x, LeftLimit.position.x, RightLimit.position.x), _PlayerTransform.position.y,
            Mathf.Clamp(_PlayerTransform.position.z, DownLimit.position.z, UpLimit.position.z));


            Ray r = new Ray(_PlayerTransform.position, Vector3.up);
          
            if (Physics.Raycast(r,out RaycastHit hit,FollowPosOffSet.y,1<<3) && !MySystem.IsInLevel())
            {
                transform.position = Vector3.Lerp(transform.position, _PlayerTransform.position +2/3f * FollowPosOffSet -Vector3.up * (transform.position.y - hit.point.y), FollowSpeed * Time.deltaTime);

            }
            else
            {
                 transform.position = Vector3.Lerp(transform.position, playerPosAfterClamp + FollowPosOffSet, FollowSpeed * Time.deltaTime);
            }
            
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position,targetPos,GoToFixedPosSpeed * Time.deltaTime);
      
        }

        //振动
        if (IsShake == true)
        {
            float t = (Mathf.PerlinNoise(0, Time.time * ShakeSpeed) - 0.5f) * ShakeStrength;
            float t2 = (Mathf.PerlinNoise(1, Time.time * ShakeSpeed) - 0.5f) * ShakeStrength;
            transform.position = transform.position + new Vector3(t, 0, t2);
        }
    }
    public void FollowPlayer(bool IsFollowPlayer)
    {
        this.IsFollowPlayer = IsFollowPlayer;
    }//让相机是否跟随玩家
    public void StartShake(float time = 0.3f, float strength = 1, float shakeSpeed = 10)
    {

        IsShake = true;
        ShakeStrength = strength * 0.1f;
        ShakeSpeed = shakeSpeed;
        MonoController.Instance.InvokeUnScaled(time, () => IsShake = false);

    }
    public void StopShake()
    {

        IsShake = false;
    }

    public void GoToPos(Vector3 TargetPos,float Speed)
    {
        
        originPos = transform.position;
        this.targetPos = TargetPos;
        GoToFixedPosSpeed = Speed;
    }
    public void BackToOriginPos(float Speed)
    {
        
        this.targetPos = originPos;
        GoToFixedPosSpeed = Speed;
    }
    public void GoToRightSide()
    {
       
        FollowPlayer(false);
         GoToPos(Camera.main.transform.position + CameraOffset, 0.8f);
       
    }
    public void ChangeRotation(Vector3 tar)
    {
        eulerAngleTarget = tar;
    }
    public void RotationBack()
    {
        eulerAngleTarget = defaulteulerAngleTarget;
    }
}
