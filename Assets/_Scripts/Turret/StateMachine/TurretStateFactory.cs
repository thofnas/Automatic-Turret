using Turret.StateMachine.States;

namespace Turret.StateMachine
{
    public class TurretStateFactory
    {
        private TurretStateMachine _context;

        public TurretStateFactory(TurretStateMachine currentContext)
        {
            _context = currentContext;
        }

        public TurretState Idle() => new TurretIdleState(_context, this);
        public TurretState Aiming() => new TurretAimingState(_context, this);
        public TurretState Shooting() => new TurretShootingState(_context, this);
        public TurretState Destroyed() => new TurretDestroyedState(_context, this);
    }
}
