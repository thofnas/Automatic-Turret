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

        public UIState UIGame() => new UIPlayState(_context, this);
        public UIState UILobby() => new UILobbyState(_context, this);
        public UIState UIWaveLost() => new UIWaveLostState(_context, this);
    }
}
