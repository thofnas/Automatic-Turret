using Events;
using Managers;

namespace Waves.StateMachine.States
{
    public class WaitingToStartWaveState : WaveState
    {
        public WaitingToStartWaveState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        
        public override void EnterState()
        {
            UIEvents.OnStartWaveButtonClicked.AddListener(UIEvents_Waves_OnStartWaveButtonClick);
        }
        public override void ExitState()
        {
            UIEvents.OnStartWaveButtonClicked.RemoveListener(UIEvents_Waves_OnStartWaveButtonClick);
            GameEvents.OnWaveStarted.Invoke();
        }
        
        public override void UpdateState() { }

        public override void CheckSwitchStates() { }
        
        private void UIEvents_Waves_OnStartWaveButtonClick() => SwitchState(Factory.SpawningEnemies());
    }
}
