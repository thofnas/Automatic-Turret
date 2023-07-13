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
        
        public static readonly EventRegister OnEnemySpotted = new();
        #endregion

        #region Enemy/Enemies
        public static readonly EventRegister<OnEnemyDestroyEventArgs> OnEnemyDestroy = new();
        public static readonly EventRegister<Guid> OnEnemyRollStart = new();
        public static readonly EventRegister<Guid> OnEnemyRollEnd = new();
        #endregion
    }
}