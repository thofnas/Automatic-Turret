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
        private Enemy.Enemy _closestEnemy;

        public override void EnterState()
        {
            RotateTowardsClosestEnemy();
            GameEvents.OnEnemyDestroyed.AddListener(GameEvents_Enemy_OnEnemyDestroyed);
        }

        public override void ExitState()
        {
            GameEvents.OnEnemyDestroyed.RemoveListener(GameEvents_Enemy_OnEnemyDestroyed);
            if (_aimRoutine != null) Ctx.StopCoroutine(_aimRoutine);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (!EnemyManager.Instance.HasEnemyInSight())
            {
                SwitchState(Factory.Idle());
                return;
            }

            if (Ctx.IsEnemyInFront(_closestEnemy) && !Ctx.IsAiming)
            {
                SwitchState(Factory.Shooting());
                return;
            }

            if (Ctx.IsDestroyed)
                SwitchState(Factory.Destroyed());
        }

        private void RotateTowardsClosestEnemy()
        {
            if (!EnemyManager.Instance.HasEnemyInSight()) return;
            
            _closestEnemy = EnemyManager.Instance.GetClosestSpottedEnemy();
            _aimRoutine = AimTurretRoutine(_closestEnemy.GetTransform().position);
            if (Ctx != null) Ctx.StartCoroutine(_aimRoutine);
        }
        
        private IEnumerator AimTurretRoutine(Vector3 targetPosition)
        {
            targetPosition = new Vector3(targetPosition.x, Ctx.transform.position.y, targetPosition.z);
            
            const float speedMultiplier = 0.25F;
            float rotationSpeed = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.RotationSpeed);
            float step = rotationSpeed * speedMultiplier * Time.deltaTime;
            Quaternion rotationTarget = Quaternion.LookRotation(targetPosition - Ctx.transform.position);

            GameEvents.TurretOnAimStart.Invoke();

            while (Quaternion.Angle(Ctx.transform.rotation, Quaternion.LookRotation(targetPosition - Ctx.transform.position)) > TOLERANCE)
            {
                Quaternion rotation = Quaternion.RotateTowards(Ctx.transform.rotation, rotationTarget, step);
                rotation.x = 0;
                rotation.z = 0;
                Ctx.transform.rotation = rotation;
                step += rotationSpeed * Time.deltaTime * speedMultiplier;

                yield return null;
            }
            
            SwitchState(Factory.Shooting());
            
            GameEvents.TurretOnAimEnd.Invoke();
        }
        private const double TOLERANCE = 0.01F;

        private void GameEvents_Enemy_OnEnemyDestroyed(Enemy.Enemy enemy)
        {
            if (EnemyManager.Instance.GetClosestSpottedEnemy() == _closestEnemy) return;
            
            if (Ctx != null) Ctx.StopCoroutine(_aimRoutine);
            RotateTowardsClosestEnemy();
        }
    }
}
