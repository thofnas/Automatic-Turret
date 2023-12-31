using System;
using CustomEventArgs;
using UnityEngine;

namespace Events {
    public abstract class GameEvents
    {
        #region Turret
        public static readonly EventRecorder<Transform> OnTurretShoot = new();
        public static readonly EventRecorder TurretOnAimStart = new();
        public static readonly EventRecorder TurretOnAimEnd = new();
        public static readonly EventRecorder TurretOnReloadStart = new(); 
        public static readonly EventRecorder TurretOnReloadEnd = new();
        public static readonly EventRecorder<bool> OnTurretDamaged = new();
        public static readonly EventRecorder OnTurretDestroyed = new();
        public static readonly EventRecorder<OnStatUpgradeEventArgs> OnTurretStatUpgraded = new();
        public static readonly EventRecorder<int> OnTurretStatsReset = new();
        #endregion

        #region Enemy/Enemies
        public static readonly EventRecorder<Enemy.Enemy> OnEnemySpawned = new();
        public static readonly EventRecorder OnAllEnemiesSpawned = new();
        public static readonly EventRecorder<Enemy.Enemy> OnEnemyDestroyed = new();
        public static readonly EventRecorder<Enemy.Enemy> OnEnemyKilled = new();
        public static readonly EventRecorder<OnEnemyDamagedEventArgs> OnEnemyDamaged = new();
        public static readonly EventRecorder<Guid> OnEnemyRollStart = new();
        public static readonly EventRecorder<Guid> OnEnemyRollEnd = new();
        public static readonly EventRecorder<Enemy.Enemy> OnEnemySpotted = new();
        public static readonly EventRecorder<Enemy.Enemy> OnEnemyLostFromView = new();
        #endregion

        #region Wave System
        public static readonly EventRecorder OnWaveStarted = new();
        public static readonly EventRecorder OnWaveEnded = new();
        public static readonly EventRecorder OnWaveWon = new();
        public static readonly EventRecorder OnWaveLost = new();
        public static readonly EventRecorder OnSubWaveStarted = new();
        public static readonly EventRecorder OnSubWaveEnded = new();
        #endregion
        
        public static readonly EventRecorder OnItemPicked = new();
        public static readonly EventRecorder OnTotalGearAmountChanged = new();
        public static readonly EventRecorder<int> OnCollectedGearAmountChanged = new();
    }
}