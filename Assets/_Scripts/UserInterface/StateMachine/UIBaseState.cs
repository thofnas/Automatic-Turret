namespace UserInterface.StateMachine
{
    public abstract class UIBaseState
    {
        public UIBaseState(UIStateMachine currentContext, UIStateFactory turretStateFactory)
        {
            Ctx = currentContext;
            Factory = turretStateFactory;
        }
        
        protected readonly UIStateMachine Ctx;
        protected readonly UIStateFactory Factory;
        
        public abstract void EnterState();
        
        public abstract void ExitState();
        
        public abstract void UpdateState();
        
        public abstract void CheckSwitchStates();

        public abstract void EnableElement();

        public abstract void DisableElement();

        protected void SwitchState(UIBaseState newState)
        {
            ExitState();

            Ctx.CurrentState.DisableElement();
            
            newState.EnterState();

            Ctx.CurrentState = newState;
            
            Ctx.CurrentState.EnableElement();
        }
    }
}
