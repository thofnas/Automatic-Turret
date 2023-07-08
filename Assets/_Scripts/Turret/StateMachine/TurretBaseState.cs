using UnityEngine;

namespace Turret.StateMachine
{
    public abstract class TurretBaseState
    {
        public abstract void EnterState(TurretStateManager turret);
        public abstract void ExitState(TurretStateManager turret);
        public abstract void UpdateState(TurretStateManager turret);
        public virtual void OnCollisionEnter(TurretStateManager turret, Collision collision) {}
    }
}
