using UnityEngine;

namespace Turret.StateMachine
{
    public class TurretStateMachine : Singleton<TurretStateMachine>
    {
        [SerializeField] private bool _isShootingLocked;
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private bool _isReloading;
        [SerializeField] private float _turretRotationSpeed = 2F;
        [SerializeField] private Transform _gunStartPoint;
        [SerializeField] private Transform _gunEndPoint;
        [SerializeField] [Range(0.1F, 4F)] private float _reloadTimeInSeconds;
        
        // state variables
        private TurretStateFactory _states;
        private TurretBaseState _currentState;
        
        // getters and setters
        public TurretBaseState CurrentState { get => _currentState; set => _currentState = value; }
        public bool IsShootingLocked { get => _isShootingLocked; set => _isShootingLocked = value; }
        public bool IsEnabled { get => _isEnabled; }
        public bool IsReloading { get => _isReloading; set => _isReloading = value; }
        public float TurretRotationSpeed { get => _turretRotationSpeed; }
        public Transform GunStartPoint { get => _gunStartPoint; }
        public Transform GunEndPoint { get => _gunEndPoint; }
        public float ReloadTimeInSeconds { get => _reloadTimeInSeconds; }

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

        public void SwitchState(TurretBaseState state)
        {
            state.ExitState();
            CurrentState = state;
            state.EnterState();
        }

        public Transform GetTransform() => transform;
    }
}
