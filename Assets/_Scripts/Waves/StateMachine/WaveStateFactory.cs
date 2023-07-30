using Waves.StateMachine.States;

namespace Waves.StateMachine
{
    public class WaveStateFactory
    {
        private WaveStateMachine _context;

        public WaveStateFactory(WaveStateMachine currentContext)
        {
            _context = currentContext;
        }

        public WaveBaseState SpawningEnemies() => new SpawningEnemiesState(_context, this);
        public WaveBaseState WaitingForDefeatedEnemies() => new WaitingForDefeatedEnemiesState(_context, this);
        public WaveBaseState WaitingForPlayer() => new WaitingToStartState(_context, this);
    }
}
