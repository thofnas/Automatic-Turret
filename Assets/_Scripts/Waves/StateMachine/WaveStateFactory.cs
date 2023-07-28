﻿using Waves.StateMachine.States;

namespace Waves.StateMachine
{
    public class WaveStateFactory
    {
        private WaveStateMachine _context;

        public WaveStateFactory(WaveStateMachine currentContext)
        {
            _context = currentContext;
        }

        public WaveBaseState Spawning() => new SpawningEnemiesState(_context, this);
        public WaveBaseState WaitingForEnemies() => new WaitingForDefeatedEnemiesState(_context, this);
        public WaveBaseState WaitingForPlayer() => new WaitingForPlayerState(_context, this);
    }
}
