using _Managers;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretIdleState : TurretBaseState
    {
        public TurretIdleState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }
        
        public override void EnterState()
        {
            Debug.Log("Entered Idle State.");
        }
        
        public override void ExitState()
        {
            Debug.Log("Leaved Idle State.");
        }
        
        public override void UpdateState()
        {
            CheckSwitchStates();
        }
        
        public override void CheckSwitchStates()
        {
            if (EnemyManager.Instance.EnemiesInSightList.Count <= 0) return;
            
            SwitchState(Factory.Shooting());
        }
        
        public override void InitializeSubState()
        {
            
        }
    }
}
