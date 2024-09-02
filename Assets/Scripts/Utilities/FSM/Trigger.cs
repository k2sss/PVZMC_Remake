using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyClass.FSM;
namespace MyClass.FSM
{
    public abstract class Trigger<T>
    {
        protected T owner;
        public Trigger(T owner)
        {
            this.owner = owner;
            Init();
        }
        public abstract void Init();
        public abstract bool TriggerHandler(FSM<T> mgr);
    }
}
