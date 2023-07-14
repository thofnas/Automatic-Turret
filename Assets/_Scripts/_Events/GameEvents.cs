using System;
using _CustomEventArgs;

namespace _Events {
    public abstract class GameEvents
    {
        #region Turret
        public static readonly EventRegister<OnShootEventArgs> TurretOnShoot = new();
        public static readonly EventRegister TurretOnAimStart = new();
        public static readonly EventRegister TurretOnAimEnd = new();
        public static readonly EventRegister TurretOnReloadStart = new(); 
        public static readonly EventRegister TurretOnReloadEnd = new();
        #endregion

        #region Enemy/Enemies
        public static readonly EventRegister<Guid> OnEnemyDestroyed = new();
        public static readonly EventRegister<Guid> OnEnemyRollStart = new();
        public static readonly EventRegister<Guid> OnEnemyRollEnd = new();
        public static readonly EventRegister<Enemy> OnEnemySpotted = new();
        #endregion
    }
}