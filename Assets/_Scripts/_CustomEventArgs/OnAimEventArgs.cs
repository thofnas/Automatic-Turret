using System;
using Turret.StateMachine;

namespace _CustomEventArgs
{
    public class OnAimEventArgs : EventArgs
    {
        public TurretStateManager Turret;
    }
}
