using Managers;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretIdleState : TurretBaseState
    {
        public TurretIdleState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }
        
        public override void EnterState() => Debug.Log("Entered Idle State.");

        public override void ExitState() { }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (EnemyManager.Instance.HasEnemyInSight())
            {
                SwitchState(Factory.Aiming());
            }
        }
        
        public override void InitializeSubState()
        {
            
        }
    }
}
