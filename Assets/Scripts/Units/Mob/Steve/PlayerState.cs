using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyClass.FSM;

namespace MyClass.FSM.Player
{
    public class PlayerState : BaseState<Mob>
    {
        public PlayerState(int StateID, Mob owner) : base(StateID, owner)
        {
        }

        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }


}



