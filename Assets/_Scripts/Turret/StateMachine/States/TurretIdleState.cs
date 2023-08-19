using Managers;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretIdleState : TurretState
    {
        public TurretIdleState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }
        
        public override void EnterState() { }

        public override void ExitState() { }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (EnemyManager.Instance.HasEnemyInSight())
            {
                SwitchState(Factory.Aiming());
            }
        }
    }
}
