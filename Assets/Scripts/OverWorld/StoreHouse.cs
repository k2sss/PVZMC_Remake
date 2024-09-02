using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OverWorld
{
    public class StoreHouse : MonoBehaviour
    {
        public float CheckRange = 3;
        private Transform PlayerTransform;
        public bool IsEnter { get; private set; }
        public Vector3 targetPos = new Vector3(26, 4, 16);
        public Vector3 targetRot = new Vector3(20, 10, 0);
        void Start()
        {
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            UIMgr.Instance.CloseUI("Store");
        }
        
        // Update is called once per frame
        void Update()
        {
            if ((PlayerTransform.position - transform.position).magnitude < CheckRange)
            {
                if (IsEnter == false)
                {
                    UIMgr.Instance.PopUI();
                    UIMgr.Instance.ShowUI("Store");
                    UIMgr.Instance.IsOpenStore = true;
                    SoundSystem.Instance.PlayRandom2Dsound("CrazyDave");
                    PhoneControlMgr.Instance.CloseJumpAndAttackButton(true);
                    CameraAction.Instance.FollowPlayer(false);
                    CameraAction.Instance.GoToPos(targetPos, 5);
                    CameraAction.Instance.ChangeRotation(targetRot);
                    IsEnter = true;
                }

            }
            else
            {
                if (IsEnter == true)
                {
                    UIMgr.Instance.IsOpenStore = false;
                    UIMgr.Instance.CloseUI("Store");
                    PhoneControlMgr.Instance.CloseJumpAndAttackButton(false);
                    IsEnter = false;
                    CameraAction.Instance.FollowPlayer(true);
                    CameraAction.Instance.RotationBack();
                }
               
            }
        }
    }
}