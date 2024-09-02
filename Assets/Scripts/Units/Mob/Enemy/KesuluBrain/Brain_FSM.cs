using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyClass.FSM;
namespace MyClass.FSM.Kesulu
{
    public class Brain_FSM<Enemy> : FSM<Enemy>
    {
        public Brain_FSM(Enemy owner) : base(owner)
        {

        }

        public enum StateType
        {
            HIDE,
            PHASE1,
            PHASE2,
            
        }

        public override void SetBase()
        {   
            //���״̬
            AddState((int)StateType.HIDE, new HIDE<Enemy>((int)StateType.HIDE, owner));
            AddState((int)StateType.PHASE1, new Phase1<Enemy>((int)StateType.PHASE1,owner));
            AddState((int)StateType.PHASE2, new Phase2<Enemy>((int)StateType.PHASE2, owner));
           
            //���ת������
            AddTrisition((int)StateType.HIDE, new Trigger_Enter<Enemy>(owner), (int)StateType.PHASE1);
            AddTrisition((int)StateType.PHASE1, new Trigger_isPhase2<Enemy>(owner), (int)StateType.PHASE2);

            //���ó�ʼ״̬
            InitDefaultState((int)StateType.HIDE);

        }

    }
}


