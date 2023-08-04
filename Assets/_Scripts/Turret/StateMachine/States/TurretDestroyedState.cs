using Events;

namespace Turret.StateMachine.States
{
    public class TurretDestroyedState : TurretBaseState
    {
        public TurretDestroyedState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }

        public override void EnterState()
        {
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnEnded);
        }

        public override void ExitState()
        {
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnEnded);
        }
        
        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }

        private void GameEvents_Wave_OnEnded()
        {
            SwitchState(Factory.Idle());
        }
    }
}
