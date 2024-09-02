using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyClass.FSM.Kesulu
{
    public class Trigger_Enter<Enemy> : Trigger<Enemy>
    {
        public Trigger_Enter(Enemy owner) : base(owner)
        {
        }

        public override void Init()
        {

        }

        public override bool TriggerHandler(FSM<Enemy> mgr)
        {
           
            if ((owner as KesuluBrain).HeartBreakCount == 3)
            {
                return true;
            }
            return false;
        }
    }
    public class Trigger_isPhase2<Enemy> : Trigger<Enemy>
    {
        public Trigger_isPhase2(Enemy owner) : base(owner)
        {

        }

        public override void Init()
        {

        }

        public override bool TriggerHandler(FSM<Enemy> mgr)
        {
            if ((owner as KesuluBrain).Health < 2500)
            {
                return true;
            }
            return false;
        }
    }
    
}
