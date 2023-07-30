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

        protected void SwitchState(TurretBaseState newState)
        {
            ExitState();
            
            newState.EnterState();

            Ctx.CurrentState = newState;
        }
    }
}
