using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyClass.FSM;
namespace MyClass.FSM
{
    public abstract class FSM<T>
    {
        
        protected Dictionary<int,BaseState<T>> FSMActDic;//×´Ì¬ÁÐ±í
        private BaseState<T> curState;
        private BaseState<T> defaultState;
        protected T owner;

        public FSM(T owner)
        {
            FSMActDic = new Dictionary<int,BaseState<T>>();
            this.owner = owner;
            SetBase();
        }
        public abstract void SetBase();
        
        public void InitDefaultState(int StateID)
        {
            if (defaultState != null) return;

            if(FSMActDic.ContainsKey(StateID))
            {
                defaultState = FSMActDic[StateID];
                curState = defaultState;
                curState.OnEnter();
            }
        }
      
        public void AddState(int stateID,BaseState<T> state)
        {
            FSMActDic.Add(stateID, state);
        }
        public void AddTrisition(int stateID,Trigger<T> trigger,int nextstateID)
        {
            if (FSMActDic.ContainsKey(stateID) && FSMActDic.ContainsKey(nextstateID))
            {
                FSMActDic[stateID].AddMap(trigger, FSMActDic[nextstateID]);
            }
        }
        public BaseState<T> GetState(int stateID)
        {
            if (FSMActDic.ContainsKey(stateID))
            {
                return FSMActDic[stateID];
            }
            Debug.Log("Cant Find stateID");
            return null;
        }
        public BaseState<T> GetCurrentState()
        {
            return curState;
        }
        public virtual void SwitchState(BaseState<T> nextState)
        {
            if (curState == nextState) return;
            curState.OnExit();
            curState = nextState;
            curState.OnEnter();
        }


        public void Update()
        {
            curState.Reason(this);
            curState.OnUpdate();
        }

    }
}

