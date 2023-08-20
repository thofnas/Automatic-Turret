using System;
using CustomEventArgs;
using Events;
using Managers;
using UnityEngine;

namespace Turret
{
    public class Turret : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private Transform _turretScanner;
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private bool _isReloading;
        [SerializeField] private bool _isAiming;
        [SerializeField] private Transform _gunEndPoint;
        [SerializeField] [Range(0.1F, 4.0F)] private float _reloadTimeInSeconds;
        #endregion
        
        #region Getters/setters
        public int TurretHealth { get; private set; }
        public bool IsDestroyed { get; private set; }
        public Transform TurretScanner { get => _turretScanner; }
        public bool IsEnabled { get => _isEnabled; }
        public bool IsReloading { get => _isReloading; set => _isReloading = value; }
        public bool IsAiming { get => _isAiming; set => _isAiming = value; }
        public Transform GunEndPoint { get => _gunEndPoint; }
        #endregion
        
        private void Awake()
        {
            ResetTurret();
            GameEvents.TurretOnAimStart.AddListener(GameEvents_Turret_OnAimStart);
            GameEvents.TurretOnAimEnd.AddListener(GameEvents_Turret_OnAimEnd);
            GameEvents.TurretOnReloadStart.AddListener(GameEvents_Turret_OnReloadStart);
            GameEvents.TurretOnReloadEnd.AddListener(GameEvents_Turret_OnReloadEnd);
            GameEvents.OnTurretDamaged.AddListener(GameEvents_Turret_OnGotHit);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnEnded);
            GameEvents.OnTurretStatUpgraded.AddListener(GameEvents_Turret_OnStatUpgraded);
        }

        private void OnDestroy()
        {
            GameEvents.TurretOnAimStart.RemoveListener(GameEvents_Turret_OnAimStart);
            GameEvents.TurretOnAimEnd.RemoveListener(GameEvents_Turret_OnAimEnd);
            GameEvents.TurretOnReloadStart.RemoveListener(GameEvents_Turret_OnReloadStart);
            GameEvents.TurretOnReloadEnd.RemoveListener(GameEvents_Turret_OnReloadEnd);
            GameEvents.OnTurretDamaged.RemoveListener(GameEvents_Turret_OnGotHit);
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnEnded);
            GameEvents.OnTurretStatUpgraded.AddListener(GameEvents_Turret_OnStatUpgraded);
        }

        private void ResetTurret()
        {
            TurretHealth = Mathf.RoundToInt(UpgradeManager.Instance.GetTurretUpgradedStat(Stat.AmountOfHealth));
            IsDestroyed = false;
        }
        
        #region Events methods
        private void GameEvents_Turret_OnAimStart() => _isAiming = true;
        
        private void GameEvents_Turret_OnAimEnd() => _isAiming = false;
        
        private void GameEvents_Turret_OnReloadStart() => _isReloading = true;
        
        private void GameEvents_Turret_OnReloadEnd() => _isReloading = false;
        
        private void GameEvents_Turret_OnGotHit(bool isABoss)
        {
            if (isABoss)
            {
                TurretHealth = 0;
                IsDestroyed = true;
                return;
            }
            
            TurretHealth--;
            if (TurretHealth <= 0) IsDestroyed = true;
        }

        private void GameEvents_Wave_OnStarted() => ResetTurret();
        
        private void GameEvents_Wave_OnEnded() => ResetTurret();

        private void GameEvents_Turret_OnStatUpgraded(OnStatUpgradeEventArgs obj) => ResetTurret();
        #endregion
    }
}
