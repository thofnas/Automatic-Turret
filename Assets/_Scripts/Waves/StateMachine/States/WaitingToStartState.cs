﻿using Events;
using Waves.StateMachine;

namespace Waves.StateMachine.States
{
    public class WaitingToStartState : WaveBaseState
    {
        public WaitingToStartState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        public override void EnterState()
        {
            UIEvents.OnStartWaveButtonClick.AddListener(UIEvents_OnStartWaveButtonClick);
        }
        public override void ExitState()
        {
            UIEvents.OnStartWaveButtonClick.RemoveListener(UIEvents_OnStartWaveButtonClick);
        }
        public override void UpdateState()
        {
        }
        public override void CheckSwitchStates()
        {
        }
        public override void InitializeSubState()
        {
        }
        
        private void UIEvents_OnStartWaveButtonClick()
        {
            SwitchState(Factory.Spawning());
        }
    }
}
