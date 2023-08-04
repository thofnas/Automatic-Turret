using UserInterface.StateMachine.States;

namespace UserInterface.StateMachine
{
    public class UIStateFactory
    {
        private UIStateMachine _context;

        public UIStateFactory(UIStateMachine currentContext)
        {
            _context = currentContext;
        }

        public UIBaseState UIGame() => new UIPlayScreenState(_context, this);
        public UIBaseState UILobby() => new UILobbyScreenState(_context, this);
    }
}
