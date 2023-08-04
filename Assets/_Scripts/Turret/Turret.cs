using Events;
using UnityEngine;

namespace Turret
{
    public class Turret : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField, Min(1)] private int _turretHealthMax = 3;
        [SerializeField] private Transform _turretScanner;
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private bool _isReloading;
        [SerializeField] private bool _isAiming;
        [SerializeField, Range(0.1F, 7.0F)] private float _turretRotationSpeed = 2F;
        [SerializeField] private Transform _gunEndPoint;
        [SerializeField] [Range(0.1F, 4.0F)] private float _reloadTimeInSeconds;
        #endregion
        
        #region Getters/setters
        public int TurretHealth { get; private set; }
        public Transform TurretScanner { get => _turretScanner; }
        public bool IsEnabled { get => _isEnabled; }
        public bool IsReloading { get => _isReloading; set => _isReloading = value; }
        public bool IsAiming { get => _isAiming; set => _isAiming = value; }
        public float TurretRotationSpeed { get => _turretRotationSpeed; }
        public Transform GunEndPoint { get => _gunEndPoint; }
        public float ReloadTimeInSeconds { get => _reloadTimeInSeconds; }
        #endregion
        
        private void Awake()
        {
            ResetTurret();
            GameEvents.TurretOnAimStart.AddListener(GameEvents_Turret_OnAimStart);
            GameEvents.TurretOnAimEnd.AddListener(GameEvents_Turret_OnAimEnd);
            GameEvents.TurretOnReloadStart.AddListener(GameEvents_Turret_OnReloadStart);
            GameEvents.TurretOnReloadEnd.AddListener(GameEvents_Turret_OnReloadEnd);
            GameEvents.OnTurretGotHit.AddListener(GameEvents_Turret_OnGotHit);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnWaveEnded);
        }

        private void OnDestroy()
        {
            GameEvents.TurretOnAimStart.RemoveListener(GameEvents_Turret_OnAimStart);
            GameEvents.TurretOnAimEnd.RemoveListener(GameEvents_Turret_OnAimEnd);
            GameEvents.TurretOnReloadStart.RemoveListener(GameEvents_Turret_OnReloadStart);
            GameEvents.TurretOnReloadEnd.RemoveListener(GameEvents_Turret_OnReloadEnd);
            GameEvents.OnTurretGotHit.RemoveListener(GameEvents_Turret_OnGotHit);
        }

        private void ResetTurret()
        {
            TurretHealth = _turretHealthMax;
        }
        
        #region Events methods
        private void GameEvents_Turret_OnAimStart() => _isAiming = true;
        
        private void GameEvents_Turret_OnAimEnd() => _isAiming = false;
        
        private void GameEvents_Turret_OnReloadStart() => _isReloading = true;
        
        private void GameEvents_Turret_OnReloadEnd() => _isReloading = false;
        
        private void GameEvents_Turret_OnGotHit() => TurretHealth--;
        
        private void GameEvents_Wave_OnWaveEnded() => ResetTurret();
        #endregion
    }
}
