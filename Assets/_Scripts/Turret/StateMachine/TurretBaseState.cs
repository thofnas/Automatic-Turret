using UnityEngine;

namespace Turret.StateMachine
{
    public abstract class TurretBaseState
    {
        public TurretBaseState(TurretStateMachine currentContext, TurretStateFactory turretStateFactory)
        {
            Ctx = currentContext;
            Factory = turretStateFactory;
        }
        
        protected readonly TurretStateMachine Ctx;
        protected readonly TurretStateFactory Factory;
        
        public abstract void EnterState();
        
        public abstract void ExitState();
        
        public abstract void UpdateState();
        
        public abstract void CheckSwitchStates();
        
        public abstract void InitializeSubState();
        
        public void UpdateStates(){}

        protected void SwitchState(TurretBaseState newState)
        {
            ExitState();
            
            newState.EnterState();

            Ctx.CurrentState = newState;
        }
        
        protected void SetSuperState(){}
        
        protected void SetSubState(){}
    }
}
