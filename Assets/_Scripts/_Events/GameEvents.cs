using System;
using _CustomEventArgs;
using Waves.StateMachine;

namespace _Events {
    public abstract class GameEvents
    {
        #region Turret
        public static readonly EventRecorder<OnShootEventArgs> TurretOnShoot = new();
        public static readonly EventRecorder TurretOnAimStart = new();
        public static readonly EventRecorder TurretOnAimEnd = new();
        public static readonly EventRecorder TurretOnReloadStart = new(); 
        public static readonly EventRecorder TurretOnReloadEnd = new();
        public static readonly EventRecorder OnTurretGotHit = new();
        public static readonly EventRecorder OnTurretDestroyed = new();
        #endregion

        #region Enemy/Enemies
        public static readonly EventRecorder<Enemy> OnEnemySpawned = new();
        public static readonly EventRecorder<Guid> OnEnemyDestroyed = new();
        public static readonly EventRecorder<Guid> OnEnemyRollStart = new();
        public static readonly EventRecorder<Guid> OnEnemyRollEnd = new();
        public static readonly EventRecorder<Enemy> OnEnemySpotted = new();
        public static readonly EventRecorder<Enemy> OnEnemyLost = new();
        #endregion

        #region Wave System
        public static readonly EventRecorder<WaveBaseState> OnWaveStateChanged = new();
        #endregion
    }
}