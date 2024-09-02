using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEvent
{


public class Level2_9 :BaseLevelEvent
{
        public GameStateMgr StateMgr;
        public Animator CameraAnimator;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            StateMgr.EnterGameDirectly = true;
        }
        void Start()
        {
            EventMgr.Instance.AddEventListener("GameStart", () => CameraAnimator.enabled = false);
            StateMgr.UnableRCard();
            CameraAnimator.enabled = true;
            MonoController.Instance.Invoke(4, () => StateMgr.StartWithNoRCard());
            UIMgr.Instance.GetUIObject("CardSlot").gameObject.SetActive(false);
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}