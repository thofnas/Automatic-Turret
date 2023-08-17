using System.Collections;
using UnityEngine;
using Events;
using Managers;

namespace Turret.StateMachine.States
{
    public class TurretShootingState : TurretState
    {
        public TurretShootingState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory) { }

        public override void EnterState()
        {
            Debug.Log("Entered Shooting State.");
        }

        public override void ExitState() { }

        public override void UpdateState()
        {
            CheckSwitchStates();
            ShootHandler();
        }
        
        public override void CheckSwitchStates()
        {
            if (!EnemyManager.Instance.HasEnemyInSight())
            {
                SwitchState(Factory.Idle());
                return;
            }

            if (!Ctx.IsEnemyInFront())
            {
                SwitchState(Factory.Aiming());
                return;
            }
            
            if (Ctx.IsDestroyed) 
                SwitchState(Factory.Destroyed());
        }

        private void ShootHandler()
        {
            if (!Ctx.IsEnabled) return;
            if (Ctx.IsReloading) return;
            if (Ctx.IsAiming) return;

            GameEvents.OnTurretShoot.Invoke(Ctx.GunEndPoint);

            Ctx.StartCoroutine(ReloadGunRoutine());
        }
        
        private IEnumerator ReloadGunRoutine()
        {
            GameEvents.TurretOnReloadStart.Invoke();

            yield return new WaitForSeconds(1f / UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ReloadSpeed));

            GameEvents.TurretOnReloadEnd.Invoke();
        }
    }
}
