using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyClass.FSM
{
    public abstract class BaseState<T>
    {
        private Dictionary<Trigger<T>, BaseState<T>> TrisitionDic = new Dictionary<Trigger<T>, BaseState<T>>();
        private List<Trigger<T>> triggers = new List<Trigger<T>>();
        public int StateID;
        public T owner;
        public BaseState(int StateID,T owner)
        {
            this.StateID = StateID;
            this.owner = owner; 
            Init();
        }
        public abstract void Init();
        //检测方法,判断是否转移到下一个状态
        public void Reason(FSM<T> FSMmgr)
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                if (triggers[i].TriggerHandler(FSMmgr))
                {
                    FSMmgr.SwitchState(TrisitionDic[triggers[0]]);
                    return;
                }
            }
        }
        public void AddMap(Trigger<T> trigger,BaseState<T> nextState)
        {
            TrisitionDic.Add(trigger, nextState);
            triggers.Add(trigger);
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();


    }

}
