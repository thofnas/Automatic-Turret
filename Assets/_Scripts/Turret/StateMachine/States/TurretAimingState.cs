using System.Collections;
using Events;
using Managers;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretAimingState : TurretState
    {
        public TurretAimingState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory)
        { }

        private IEnumerator _aimRoutine;
        private Enemy _closestEnemy;

        public override void EnterState()
        {
            Debug.Log("Entered Aiming State.");
            RotateTowardsClosestEnemy();
            GameEvents.TurretOnAimStart.Invoke();
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnEnemyDestroyed);
        }

        public override void ExitState()
        {
            GameEvents.TurretOnAimEnd.Invoke();
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnEnemyDestroyed);
            if (_aimRoutine != null) Ctx.StopCoroutine(_aimRoutine);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (!EnemyManager.Instance.HasEnemyInSight())
                SwitchState(Factory.Idle());

            if (Ctx.IsEnemyInFront(_closestEnemy) && !Ctx.IsAiming)
                SwitchState(Factory.Shooting());

            if (Ctx.IsDestroyed)
                SwitchState(Factory.Destroyed());
        }

        private void RotateTowardsClosestEnemy()
        {
            if (!EnemyManager.Instance.HasEnemyInSight()) return;
            
            _closestEnemy = EnemyManager.Instance.GetClosestSpottedEnemy();
            _aimRoutine = AimTurretRoutine(_closestEnemy.GetTransform());
            if (Ctx != null) Ctx.StartCoroutine(_aimRoutine);
        }
        
        private IEnumerator AimTurretRoutine(Transform target)
        {
            const float speedMultiplier = 0.25F;
            float rotationSpeed = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.RotationSpeed);
            float step = rotationSpeed * speedMultiplier * Time.deltaTime;
            Quaternion rotationTarget = Quaternion.LookRotation(target.position - Ctx.transform.position);

            while (Quaternion.Angle(Ctx.transform.rotation, Quaternion.LookRotation(target.position - Ctx.transform.position)) > TOLERANCE)
            {
                Quaternion rotation = Quaternion.RotateTowards(Ctx.transform.rotation, rotationTarget, step);
                rotation.x = 0;
                rotation.z = 0;
                Ctx.transform.rotation = rotation;
                step += rotationSpeed * Time.deltaTime * speedMultiplier;

                yield return null;
            }

            GameEvents.TurretOnAimEnd.Invoke();
        }
        private const double TOLERANCE = 0.01F;

        private void GameEvents_Enemy_OnEnemyDestroyed(Enemy enemy)
        {
            if (EnemyManager.Instance.GetClosestSpottedEnemy() == _closestEnemy) return;
            
            if (Ctx != null) Ctx.StopCoroutine(_aimRoutine);
            RotateTowardsClosestEnemy();
        }
    }
}
