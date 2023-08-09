using Events;

namespace UserInterface.StateMachine.States
{
    public class UIWaveLostState : UIState
    {
        public UIWaveLostState(UIStateMachine currentContext, UIStateFactory turretStateFactory) : base(currentContext, turretStateFactory) { }
        public override void EnterState()
        {
            
        }
        
        public override void ExitState()
        {
        }
        
        public override void UpdateState() => CheckSwitchStates();
        
        public override void CheckSwitchStates() { }

        public override void EnableElement()
        {
            
        }
        
        public override void DisableElement()
        {
            
        }
    }
}
