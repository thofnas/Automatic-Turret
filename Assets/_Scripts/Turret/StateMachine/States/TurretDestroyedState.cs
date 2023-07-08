using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretDestroyedState : TurretBaseState
    {
        public TurretDestroyedState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }

        public override void EnterState()
        {
        }
        public override void ExitState()
        {
        }
        public override void UpdateState()
        {
            CheckSwitchStates();
        }
        public override void CheckSwitchStates()
        {
        }
        public override void InitializeSubState()
        {
        }
    }
}
