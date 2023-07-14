using System;
using System.Collections;
using UnityEngine;
using _CustomEventArgs;
using _Events;
using _Managers;

namespace Turret.StateMachine.States
{
    public class TurretShootingState : TurretBaseState
    {
        public TurretShootingState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }

        public override void EnterState()
        {
            Debug.Log("Entered Shooting State.");
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.AddListener(GameEvents_Enemy_OnEnemySpotted);
        }

        public override void ExitState()
        {
            Debug.Log("Leaved Shooting State.");
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnEnemyDestroyed);
            GameEvents.OnEnemySpotted.RemoveListener(GameEvents_Enemy_OnEnemySpotted);
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
            if (!Ctx.IsEnabled) return;
            if (Ctx.IsReloading) return;
            if (Ctx.IsShootingLocked) return;
        
            GameEvents.TurretOnShoot.Invoke(new OnShootEventArgs {
                GunEndPointPosition = Ctx.GunEndPoint.position,
                GunStartPointPosition = Ctx.GunStartPoint.position,
            });

            Ctx.StartCoroutine(ReloadGunRoutine());
        }
        
        public void RotateTowardsClosestEnemy()
        { 
            if (EnemyManager.Instance.EnemiesInSightList.Count <= 0) return;
            
            Debug.Log(EnemyManager.Instance.GetClosestEnemy().transform);
            Ctx.StartCoroutine(AimTurretRoutine(EnemyManager.Instance.GetClosestEnemy().transform));
        }
        
        public IEnumerator AimTurretRoutine(Transform target)
        {
            const float speedMultiplier = 0.25F;
            float step = Ctx.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            Quaternion rotationTarget = Quaternion.LookRotation(target.position - Ctx.transform.position);

            Ctx.IsShootingLocked = true;

            GameEvents.TurretOnAimStart.Invoke();
            
            while (rotationTarget != Ctx.transform.rotation)
            {
                Ctx.transform.rotation = Quaternion.RotateTowards(Ctx.transform.rotation, rotationTarget, step);
                step += Ctx.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            
                yield return null;
            }
            
            Ctx.IsShootingLocked = false;
            
            GameEvents.TurretOnAimEnd.Invoke();
        }
        
        public IEnumerator ReloadGunRoutine()
        {
            GameEvents.TurretOnReloadStart.Invoke();
            
            Ctx.IsReloading = true;
            
            yield return new WaitForSeconds(Ctx.ReloadTimeInSeconds);
        
            Ctx.IsReloading = false;
            
            GameEvents.TurretOnReloadEnd.Invoke();
        }

        private void GameEvents_Enemy_OnEnemyDestroyed(Guid enemyID)
        {
            RotateTowardsClosestEnemy();
        }

        private void GameEvents_Enemy_OnEnemySpotted(Enemy enemy)
        {
            RotateTowardsClosestEnemy();
        }
    }
}
