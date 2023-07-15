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

        public TurretBaseState Idle() => new TurretIdleState(_context, this);
        public TurretBaseState Aiming() => new TurretAimingState(_context, this);
        public TurretBaseState Shooting() => new TurretShootingState(_context, this);
        public TurretBaseState Destroyed() => new TurretDestroyedState(_context, this);
    }
}
