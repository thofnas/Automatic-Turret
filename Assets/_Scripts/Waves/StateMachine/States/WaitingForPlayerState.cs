﻿using Waves.StateMachine;

namespace Waves.StateMachine.States
{
    public class WaitingForPlayerState : WaveBaseState
    {
        public WaitingForPlayerState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }
        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }
        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }
        public override void CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }
        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }
    }
}
