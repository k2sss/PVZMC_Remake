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
            //添加状态
            AddState((int)StateType.HIDE, new HIDE<Enemy>((int)StateType.HIDE, owner));
            AddState((int)StateType.PHASE1, new Phase1<Enemy>((int)StateType.PHASE1,owner));
            AddState((int)StateType.PHASE2, new Phase2<Enemy>((int)StateType.PHASE2, owner));
           
            //添加转换条件
            AddTrisition((int)StateType.HIDE, new Trigger_Enter<Enemy>(owner), (int)StateType.PHASE1);
            AddTrisition((int)StateType.PHASE1, new Trigger_isPhase2<Enemy>(owner), (int)StateType.PHASE2);

            //设置初始状态
            InitDefaultState((int)StateType.HIDE);

        }

    }
}


