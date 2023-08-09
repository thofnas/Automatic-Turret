using Events;
using Managers;

namespace Waves.StateMachine.States
{
    public class WaitingToFinishWaveState : WaveState
    {
        public WaitingToFinishWaveState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }

        public override void EnterState() { }

        public override void ExitState()
        {
            GameEvents.OnWaveEnded.Invoke();
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }
    }
}
