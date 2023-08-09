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

        public WaveState SpawningEnemies() => new SpawningEnemiesState(_context, this);
        public WaveState WaitingForDefeatedEnemies() => new WaitingForDefeatedEnemiesState(_context, this);
        public WaveState WaitingToStartWave() => new WaitingToStartWaveState(_context, this);
        public WaveState WaitingToFinishWave() => new WaitingToFinishWaveState(_context, this);
    }
}
