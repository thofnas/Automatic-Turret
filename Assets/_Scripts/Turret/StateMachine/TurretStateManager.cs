using System;
using _CustomEventArgs;
using Turret.StateMachine.States;
using UnityEngine;

namespace Turret.StateMachine
{
    public class TurretStateManager : Singleton<TurretStateManager>
    {
        public bool IsShootingLocked;
        public bool IsEnabled = true;
        public float TurretRotationSpeed = 2F;
        public Transform GunStartPoint;
        public Transform GunEndPoint;
        [Range(0.1F, 4F)] public float ReloadTimeInSeconds;
        public bool IsReloading;
        
        private TurretBaseState _currentState;
        public readonly TurretIdleState IdleState = new();
        public readonly TurretShootingState ShootingState = new();
        public readonly TurretDestroyedState DestroyedState = new();

        private void Start()
        {
            _currentState = IdleState;
            _currentState.EnterState(this);
        }

        private void Update()
        {
            _currentState.UpdateState(this);
        }

        public void SwitchState(TurretBaseState state)
        {
            state.ExitState(this);
            _currentState = state;
            state.EnterState(this);
        }

        private void OnEnable()
        {
            TurretShootingState.OnAimStart += TurretShootingState_OnAimStart;
            TurretShootingState.OnReloadStart += TurretShootingState_OnReloadStart;
        }
        
        private void OnDisable()
        {
            TurretShootingState.OnAimStart -= TurretShootingState_OnAimStart;
            TurretShootingState.OnReloadStart -= TurretShootingState_OnReloadStart;
        }
        
        private void TurretShootingState_OnAimStart(object sender, OnAimEventArgs e)
        {
            StartCoroutine(TurretShootingState.AimTurretRoutine(this, TurretScanner.Instance.EnemyList.Peek()));
        }
        
        private void TurretShootingState_OnReloadStart(object sender, EventArgs e)
        {
            StartCoroutine(TurretShootingState.ReloadGunRoutine());
        }
        
        public TurretStateManager GetTurret() => this;
    }
}
