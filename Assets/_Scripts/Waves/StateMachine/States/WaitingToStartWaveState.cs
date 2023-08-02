using Events;

namespace Waves.StateMachine.States
{
    public class WaitingToStartWaveState : WaveBaseState
    {
        public WaitingToStartWaveState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        public override void EnterState()
        {
            GameEvents.OnWaveEnded.Invoke();
            UIEvents.OnStartWaveButtonClick.AddListener(UIEvents_Waves_OnStartWaveButtonClick);
        }
        public override void ExitState()
        {
            UIEvents.OnStartWaveButtonClick.RemoveListener(UIEvents_Waves_OnStartWaveButtonClick);
            GameEvents.OnWaveStarted.Invoke();
        }
        
        public override void UpdateState() { }

        public override void CheckSwitchStates() { }
        
        private void UIEvents_Waves_OnStartWaveButtonClick()
        {
            SwitchState(Factory.SpawningEnemies());
        }
    }
}
