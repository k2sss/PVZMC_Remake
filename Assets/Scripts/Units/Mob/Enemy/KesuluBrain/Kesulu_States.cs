using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyClass.Tools;
using DG.Tweening;
using MyClass.Extensions.Vector3Extension;

namespace MyClass.FSM.Kesulu
{
    public class HIDE<Enemy> : BaseState<Enemy>
    {

        public HIDE(int StateID, Enemy owner) : base(StateID, owner)
        {

        }

        public override void Init()
        {

        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            (owner as KesuluBrain).EnterScene();
        }

        public override void OnUpdate()
        {

        }
    }
    public class Phase1<Enemy> : BaseState<Enemy>
    {
        public enum ActionType
        {
            IDLE,
            CALLENEMY,
            SENDEYE,
            BOMB,
            DOWN,
        }
        public ActionType currentType;
        public Dictionary<ActionType, System.Action> ActionDic;
        public int[] ActionWeights;
        private float Phase1Timer;
        private KesuluBrain brain;
        public Phase1(int StateID, Enemy owner) : base(StateID, owner)
        {

        }

        public override void Init()
        {
            brain = owner as KesuluBrain;
            ActionDic = new Dictionary<ActionType, System.Action>
            {
                { ActionType.IDLE, () => { brain.thisAnimator.SetInteger("Action",0); } },
                { ActionType.CALLENEMY, () => { brain.thisAnimator.SetInteger("Action",1); } },
                { ActionType.SENDEYE, () => {  brain.thisAnimator.SetInteger("Action",3);} },
                { ActionType.BOMB, () => {  brain.thisAnimator.SetInteger("Action",2);} },
                { ActionType.DOWN, () => { brain.thisAnimator.SetInteger("Action",4); } }
            };
            ActionWeights = new int[]
            {
                0,
                30,
                30,
                20,
                0
            };

            currentType = ActionType.IDLE;

        }
        private void ChangeMotion(ActionType actType)
        {
            if (currentType == actType) return;
            currentType = actType;
            ActionDic[actType]?.Invoke();
        }

        public void NextMotion()
        {
            switch (currentType)
            {
                case ActionType.IDLE:
                    ActionWeights[2] = (brain.IsTargetFarAway()) ? 35 : 20;
                    ActionWeights[4] = (brain.IsTargetNearBy()) ? 200 : 0;
                    ChangeMotion((ActionType)RanTool.GetWeightResult(ActionWeights));
                    break;
                case ActionType.CALLENEMY:
                    ChangeMotion(ActionType.SENDEYE);
                    break;
                default:
                    ChangeMotion(ActionType.IDLE);
                    break;
            }
            Phase1Timer = 0;

        }
        public override void OnEnter()
        {
            LevelManager.Instance.nowWave = 12;
        }

        public override void OnExit()
        {


        }

        public override void OnUpdate()
        {

            Phase1Timer += Time.deltaTime;
            if (Phase1Timer > 12)
            {
                Phase1Timer = 0;
                NextMotion();
            }
        }
    }
    public class Phase2<Enemy> : BaseState<Enemy>
    {
        public enum ActionType
        {
            IDLE,
            SENDEYE,
            BOMB,
            DOWN,
            LASER,
            BUFF,
            CRIMSON_BREATH,
            MOVE,
            MOVE2TARGET,
        }
        public ActionType currentType;
        public Dictionary<ActionType, System.Action> ActionDic;
        public int[] ActionWeights;
        private float PhaseTimer;
        private KesuluBrain brain;
        public Phase2(int StateID, Enemy owner) : base(StateID, owner)
        {

        }

        public override void Init()
        {
            brain = owner as KesuluBrain;
            ActionDic = new Dictionary<ActionType, System.Action>
            {
                { ActionType.IDLE, () => { 
                    brain.thisAnimator.SetInteger("Action",0);
                    MoveToOriginPos();
                } },
                { ActionType.SENDEYE, () => { brain.thisAnimator.SetInteger("Action",1); } },
                { ActionType.BOMB, () => { brain.thisAnimator.SetInteger("Action",3); } },
                { ActionType.DOWN, () => { brain.thisAnimator.SetInteger("Action",2); } },
                { ActionType.LASER, () => { brain.thisAnimator.SetInteger("Action",4); } },
                { ActionType.BUFF, () => { brain.thisAnimator.SetInteger("Action",6); } },
                { ActionType.CRIMSON_BREATH, () => { brain.thisAnimator.SetInteger("Action",5); } },
                { ActionType.MOVE, () => { MoveToTargetLine(); } },
                { ActionType.MOVE2TARGET, () => { MoveToTarget(); } },
            };
            ActionWeights = new int[]
            {
                0,
                15,
                20,
                0,
                20,
                20,
                20,
                20,
                20,
            };

            currentType = ActionType.IDLE;

        }
        private void ChangeMotion(ActionType actType)
        {
            if (currentType == actType) return;
            currentType = actType;
            ActionDic[actType]?.Invoke();
        }

        public void NextMotion()
        {
            switch (currentType)
            {
                case ActionType.IDLE:

                    ActionWeights[3] = (brain.IsTargetNearBy()) ? 200 : 0;
                    bool isForward = brain.IsTargetForward();
                    ActionWeights[4] = isForward ? 30 : 0;
                    ActionWeights[6] = isForward ? 30 : 0;
                    ActionWeights[7] = isForward ? 0 :  40;
                    ActionWeights[8] = isForward ? 20 : 30;
                   ChangeMotion((ActionType)RanTool.GetWeightResult(ActionWeights));
                    break;

                case ActionType.MOVE2TARGET:
                    ChangeMotion(ActionType.DOWN);
                    break;
                case ActionType.MOVE:

                    if(Random.Range(0,100)<50)
                    ChangeMotion(ActionType.CRIMSON_BREATH);
                    else
                    ChangeMotion(ActionType.LASER);
                    break;
                default:
                    ChangeMotion(ActionType.IDLE);
                    break;
            }
            PhaseTimer = 0;

        }
        public override void OnEnter()
        {
            brain.ChangeAnimatorControllerToPhase2();
        }

        public override void OnExit()
        {


        }

        public override void OnUpdate()
        {

            PhaseTimer += Time.deltaTime;
            if (PhaseTimer > 8)
            {
                PhaseTimer = 0;
                NextMotion();
            }
        }
        public void MoveToOriginPos()
        {
            brain.transform.DOMove(brain.GetOriginPos(),1);
        }
        public void MoveToTargetLine()
        {
            Vector3 targetPos = brain.GetTarget().transform.position;
            Vector3 origin = brain.GetOriginPos();
            float finalz = GridManager.Instance.TransToGridZAxis(targetPos.z);
            Vector3 final = new Vector3(origin.x, origin.y, finalz);
            brain.transform.DOMove(final,1);
            MonoController.Instance.Invoke(1.1f,()=>NextMotion());
        }
        public void MoveToTarget()
        {
            
            Vector3 targetPos = brain.GetTarget().transform.position;
            Vector3 origin = brain.GetOriginPos();
            Vector3 final = new Vector3(targetPos.x, origin.y, targetPos.z);
            final = final.ClampIgnoreYAxis(GridManager.Instance.transform.position, GridManager.Instance.GetMaxPos());
            brain.transform.DOMove(final, 1);
            MonoController.Instance.Invoke(1.1f, () => NextMotion());
        }
    }
   
}
