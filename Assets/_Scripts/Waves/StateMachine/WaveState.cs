namespace Waves.StateMachine
{
    public abstract class WaveState
    {
        public WaveState(WaveStateMachine currentContext, WaveStateFactory turretStateFactory)
        {
            Ctx = currentContext;
            Factory = turretStateFactory;
        }
        
        protected readonly WaveStateMachine Ctx;
        protected readonly WaveStateFactory Factory;

        public abstract void EnterState();

        public abstract void ExitState();
        
        public abstract void UpdateState();
        
        public abstract void CheckSwitchStates();
        
        public void UpdateStates(){}

        protected void SwitchState(WaveState newState)
        {
            ExitState();
            
            newState.EnterState();

            Ctx.CurrentState = newState;
        }
    }
}
