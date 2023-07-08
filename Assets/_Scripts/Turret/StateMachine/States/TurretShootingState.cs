using System;
using System.Collections;
using _CustomEventArgs;
using _Managers;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretShootingState : TurretBaseState
    {
        public TurretShootingState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }
        
        //events
        public static event EventHandler<OnShootEventArgs> OnShoot;
        public static event Action OnAimStart;
        public static event Action OnAimEnd;
        public static event Action OnReloadStart; 
        public static event Action OnReloadEnd;
        
        public override void EnterState()
        {
            Debug.Log("Entered Shooting State.");
            // Enemy.OnEnemyDestroyEvent += EnemyOnEnemyDestroyEvent;
            TurretScanner.Instance.OnEnemySpotted += TurretScanner_OnEnemySpotted;
        }
        
        public override void ExitState()
        {
            Debug.Log("Leaved Shooting State.");
            // Enemy.OnEnemyDestroyEvent -= EnemyOnEnemyDestroyEvent;
            TurretScanner.Instance.OnEnemySpotted -= TurretScanner_OnEnemySpotted;
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            ShootHandler();
        }
        
        public override void CheckSwitchStates()
        {
            if (EnemyManager.Instance.EnemiesInSightList.Count > 0) return;
            SwitchState(Factory.Idle());
        }
        
        public override void InitializeSubState()
        {
        }

        public void ShootHandler()
        {
            Debug.Log(Ctx.IsReloading);

            if (!Ctx.IsEnabled) return;
            if (Ctx.IsReloading) return;
            if (Ctx.IsShootingLocked) return;
        
            OnShoot?.Invoke(this, new OnShootEventArgs {
                GunEndPointPosition = Ctx.GunEndPoint.position,
                GunStartPointPosition = Ctx.GunStartPoint.position,
            });

            Ctx.StartCoroutine(ReloadGunRoutine());
        }
        
        public void RotateTowardsClosestEnemy()
        { 
            if (EnemyManager.Instance.EnemiesInSightList.Count <= 0) return;

            Ctx.StartCoroutine(AimTurretRoutine(EnemyManager.Instance.EnemiesInSightList[0].transform));
        }
        
        public IEnumerator AimTurretRoutine(Transform target)
        {
            const float speedMultiplier = 0.25F;
            float step = Ctx.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            Quaternion rotationTarget = Quaternion.LookRotation(target.position - Ctx.transform.position);
            
            Ctx.IsShootingLocked = true;
            
            Debug.Log(target.position);
            
            OnAimStart?.Invoke();
            
            while (rotationTarget != Ctx.transform.rotation)
            {
                Ctx.transform.rotation = Quaternion.RotateTowards(Ctx.transform.rotation, rotationTarget, step);
                step += Ctx.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            
                yield return null;
            }
            
            Ctx.IsShootingLocked = false;
            
            OnAimEnd?.Invoke();
        }
        
        public IEnumerator ReloadGunRoutine()
        {
            OnReloadStart?.Invoke();
            
            Ctx.IsReloading = true;
            
            yield return new WaitForSeconds(Ctx.ReloadTimeInSeconds);
        
            Ctx.IsReloading = false;
            
            OnReloadEnd?.Invoke();
        }

        private void EnemyOnEnemyDestroyEvent()
        {
            RotateTowardsClosestEnemy();
        }

        private void TurretScanner_OnEnemySpotted(object sender, EventArgs e)
        {
            RotateTowardsClosestEnemy();
        }
    }
}
