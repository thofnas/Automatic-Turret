using Events;
using UnityEngine;

namespace Turret
{
    public class Turret : MonoBehaviour
    {
        #region Serialized Variables
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private bool _isReloading;
        [SerializeField] private bool _isAiming;
        [SerializeField, Range(0.1F, 7.0F)] private float _turretRotationSpeed = 2F;
        [SerializeField] private Transform _gunStartPoint;
        [SerializeField] private Transform _gunEndPoint;
        [SerializeField] [Range(0.1F, 4.0F)] private float _reloadTimeInSeconds;
        #endregion
        
        #region Getters/setters
        public bool IsEnabled { get => _isEnabled; }
        public bool IsReloading { get => _isReloading; set => _isReloading = value; }
        public bool IsAiming { get => _isAiming; set => _isAiming = value; }
        public float TurretRotationSpeed { get => _turretRotationSpeed; }
        public Transform GunStartPoint { get => _gunStartPoint; }
        public Transform GunEndPoint { get => _gunEndPoint; }
        public float ReloadTimeInSeconds { get => _reloadTimeInSeconds; }
        #endregion
        
        private void OnEnable()
        {
            GameEvents.TurretOnAimStart.AddListener(GameEvents_Turret_OnAimStart);
            GameEvents.TurretOnAimEnd.AddListener(GameEvents_Turret_OnAimEnd);
            GameEvents.TurretOnReloadStart.AddListener(GameEvents_Turret_OnReloadStart);
            GameEvents.TurretOnReloadEnd.AddListener(GameEvents_Turret_OnReloadEnd);
        }

        private void OnDisable()
        {
            GameEvents.TurretOnAimStart.RemoveListener(GameEvents_Turret_OnAimStart);
            GameEvents.TurretOnAimEnd.RemoveListener(GameEvents_Turret_OnAimEnd);
            GameEvents.TurretOnReloadStart.RemoveListener(GameEvents_Turret_OnReloadStart);
            GameEvents.TurretOnReloadEnd.RemoveListener(GameEvents_Turret_OnReloadEnd);
        }
        
        #region Events methods
        private void GameEvents_Turret_OnAimStart() => _isAiming = true;
        
        private void GameEvents_Turret_OnAimEnd() => _isAiming = false;
        
        private void GameEvents_Turret_OnReloadStart() => _isReloading = true;
        
        private void GameEvents_Turret_OnReloadEnd() => _isReloading = false;
        #endregion
    }
}
