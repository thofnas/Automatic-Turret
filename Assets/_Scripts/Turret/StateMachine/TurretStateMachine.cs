using System;
using System.Linq;
using _Events;
using _Interfaces;
using _Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Turret.StateMachine
{
    public class TurretStateMachine : Singleton<TurretStateMachine>
    {
        #region Serialized Variables
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private bool _isReloading;
        [SerializeField] private bool _isAiming;
        [SerializeField] private float _turretRotationSpeed = 2F;
        [SerializeField] private Transform _gunStartPoint;
        [SerializeField] private Transform _gunEndPoint;
        [SerializeField] [Range(0.1F, 4F)] private float _reloadTimeInSeconds;
        #endregion
        
        // state variables
        private TurretStateFactory _states;
        private TurretBaseState _currentState;

        #region Getters/setters
        public TurretBaseState CurrentState { get => _currentState; set => _currentState = value; }
        public bool IsEnabled { get => _isEnabled; }
        public bool IsReloading { get => _isReloading; set => _isReloading = value; }
        public bool IsAiming { get => _isAiming; set => _isAiming = value; }
        public float TurretRotationSpeed { get => _turretRotationSpeed; }
        public Transform GunStartPoint { get => _gunStartPoint; }
        public Transform GunEndPoint { get => _gunEndPoint; }
        public float ReloadTimeInSeconds { get => _reloadTimeInSeconds; }
        #endregion
        
        #region Unity methods
        protected override void Awake()
        {
            base.Awake();
            _states = new TurretStateFactory(this);
        }

        private void Start()
        {
            CurrentState = _states.Idle();
            CurrentState.EnterState();
        }

        private void Update()
        {
            CurrentState.UpdateState();
        }

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
        #endregion

        public bool IsEnemyInFront(Enemy enemy)
        {
            Vector3 direction = transform.forward; // Get the forward direction of the turret
            var turretScannerCollider = TurretScanner.Instance.GetComponent<Collider>();
            float maxDistance = turretScannerCollider.bounds.extents.magnitude; // Set the maximum distance to cast the ray

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, maxDistance);

            return hits.Any(hit => hit.collider.GetComponent<Enemy>() == enemy);
        }

        public Transform GetTransform() => transform;

        #region Events methods
        private void GameEvents_Turret_OnAimStart() => _isAiming = true;
        
        private void GameEvents_Turret_OnAimEnd() => _isAiming = false;
        
        private void GameEvents_Turret_OnReloadStart() => _isReloading = true;
        
        private void GameEvents_Turret_OnReloadEnd() => _isReloading = false;
        #endregion
    }
}
