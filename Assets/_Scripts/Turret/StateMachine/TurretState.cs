namespace Turret.StateMachine
{
    public abstract class TurretState
    {
        public TurretState(TurretStateMachine currentContext, TurretStateFactory turretStateFactory)
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

        protected void SwitchState(TurretState newState)
        {
            ExitState();
            
            newState.EnterState();

            Ctx.CurrentState = newState;
        }
    }
}
