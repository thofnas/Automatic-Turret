using System;
using CustomEventArgs;
using UnityEngine;

namespace Events {
    public abstract class GameEvents
    {
        #region Turret
        public static readonly EventRecorder<Transform> TurretOnShoot = new();
        public static readonly EventRecorder TurretOnAimStart = new();
        public static readonly EventRecorder TurretOnAimEnd = new();
        public static readonly EventRecorder TurretOnReloadStart = new(); 
        public static readonly EventRecorder TurretOnReloadEnd = new();
        public static readonly EventRecorder OnTurretDamaged = new();
        public static readonly EventRecorder OnTurretDestroyed = new();
        public static readonly EventRecorder<OnStatUpgradeEventArgs> OnTurretStatUpgraded = new();
        #endregion

        #region Enemy/Enemies
        public static readonly EventRecorder<Enemy.Enemy> OnEnemySpawned = new();
        public static readonly EventRecorder OnAllEnemiesSpawned = new();
        public static readonly EventRecorder<Enemy.Enemy> OnEnemyDestroyed = new();
        public static readonly EventRecorder<Enemy.Enemy> OnEnemyDamaged = new();
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
        public static readonly EventRecorder<Item.Item> OnItemPickUpAnimationCompleted = new();
        public static readonly EventRecorder OnTotalGearAmountChanged = new();
        public static readonly EventRecorder OnCollectedGearAmountChanged = new();
    }
}