using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretIdleState : TurretBaseState
    {
        public override void EnterState(TurretStateManager turret)
        {
            Debug.Log("Entered Idle State.");
        }
        
        public override void ExitState(TurretStateManager turret)
        {
            Debug.Log("Leaved Idle State.");
        }
        
        public override void UpdateState(TurretStateManager turret)
        {
            if (TurretScanner.Instance.EnemyList.Count > 0)
            {
                turret.SwitchState(turret.ShootingState);
                return;
            }
        }
    }
}
