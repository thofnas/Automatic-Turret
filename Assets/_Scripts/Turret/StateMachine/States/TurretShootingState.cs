using System;
using System.Collections;
using _CustomEventArgs;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretShootingState : TurretBaseState
    {
        public static event EventHandler<OnShootEventArgs> OnShoot;
        public static event EventHandler OnReloadStart; 
        public static event EventHandler OnReloadEnd;
        public static event EventHandler<OnAimEventArgs> OnAimStart;
        
        private TurretStateManager _turret;

        public override void EnterState(TurretStateManager turret)
        {
            Debug.Log("Entered Shooting State.");
            Enemy.OnDestroyEvent += Enemy_OnDestroyEvent;
            TurretScanner.OnEnemySpotted += TurretScanner_OnEnemySpotted;

            _turret = turret;
        }
        
        public override void ExitState(TurretStateManager turret)
        {
            Debug.Log("Leaved Shooting State.");
            Enemy.OnDestroyEvent -= Enemy_OnDestroyEvent;
            TurretScanner.OnEnemySpotted -= TurretScanner_OnEnemySpotted;

            _turret = null;
        }

        public override void UpdateState(TurretStateManager turret)
        {
            if (TurretScanner.Instance.EnemyList.Count <= 0)
            {
                turret.SwitchState(turret.IdleState);
                return;
            }
            
            ShootHandler();
        }
        
        public void ShootHandler()
        {
            if (!TurretStateManager.Instance.IsEnabled) return;
            if (TurretStateManager.Instance.IsReloading) return;
            if (TurretStateManager.Instance.IsShootingLocked) return;
        
            OnShoot?.Invoke(this, new OnShootEventArgs {
                GunEndPointPosition = TurretStateManager.Instance.GunEndPoint.position,
                GunStartPointPosition = TurretStateManager.Instance.GunStartPoint.position,
            });
            
            OnReloadStart?.Invoke(this, EventArgs.Empty);
        }
        
        public void RotateTowardsClosestEnemy(TurretStateManager turret)
        {
            if (TurretScanner.Instance.EnemyList.Count <= 0) return;
            
            OnAimStart?.Invoke(this,new OnAimEventArgs {
                Turret = turret
            });
        }
        
        public static IEnumerator AimTurretRoutine(Component turret, Transform target)
        {
            const float speedMultiplier = 0.25F;
            float step = TurretStateManager.Instance.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            Quaternion rotationTarget = Quaternion.LookRotation(target.position - turret.transform.position);

            TurretStateManager.Instance.IsShootingLocked = true;

            while (rotationTarget != turret.transform.rotation)
            {
                turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, rotationTarget, step);
                step += TurretStateManager.Instance.TurretRotationSpeed * Time.deltaTime * speedMultiplier;

                yield return null;
            }

            TurretStateManager.Instance.IsShootingLocked = false;
        }
        
        public static IEnumerator ReloadGunRoutine()
        {
            TurretStateManager.Instance.IsReloading = true;
            
            yield return new WaitForSeconds(TurretStateManager.Instance.ReloadTimeInSeconds);
        
            TurretStateManager.Instance.IsReloading = false;
        }
        
        private void Enemy_OnDestroyEvent()
        {
            RotateTowardsClosestEnemy(_turret);
        }

        private void TurretScanner_OnEnemySpotted(object sender, EventArgs e)
        {
            RotateTowardsClosestEnemy(_turret);
        }
    }
}
