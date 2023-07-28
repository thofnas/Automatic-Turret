using System;
using System.Collections;
using _Events;
using _Interfaces;
using _Managers;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretAimingState : TurretBaseState
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
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (!EnemyManager.Instance.HasEnemyInSight())
            {
                SwitchState(Factory.Idle());
            }

            if (Ctx.IsEnemyInFront(_closestEnemy) && !Ctx.IsAiming)
            {
                SwitchState(Factory.Shooting());
            }

            // if (Ctx.IsDestroyed)
            // {
            //     SwitchState(Factory.Destroyed());
            //     return;
            // }
        }
        
        public override void InitializeSubState() { }
        
        private void RotateTowardsClosestEnemy()
        {
            if (!EnemyManager.Instance.HasEnemyInSight()) return;
            _closestEnemy = EnemyManager.Instance.GetClosestSpottedEnemy();
            _aimRoutine = AimTurretRoutine(_closestEnemy.GetTransform());
            Ctx.StartCoroutine(_aimRoutine);
        }
        
        private IEnumerator AimTurretRoutine(Transform target)
        {
            const float speedMultiplier = 0.25F;
            float step = Ctx.TurretRotationSpeed * speedMultiplier * Time.deltaTime;
            Quaternion rotationTarget = Quaternion.LookRotation(target.position - Ctx.transform.position);

            while (Math.Abs(rotationTarget.eulerAngles.y - Ctx.transform.rotation.eulerAngles.y) > TOLERANCE)
            {
                Quaternion rotation = Quaternion.RotateTowards(Ctx.transform.rotation, rotationTarget, step);
                rotation.x = 0;
                rotation.z = 0;
                Ctx.transform.rotation = rotation;
                step += Ctx.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            
                yield return null;
            }
            
            GameEvents.TurretOnAimEnd.Invoke();
        }
        private const double TOLERANCE = 0.01F;

        private void GameEvents_Enemy_OnEnemyDestroyed(Guid obj)
        {
            if (EnemyManager.Instance.GetClosestSpottedEnemy() == _closestEnemy) return;
            Ctx.StopCoroutine(_aimRoutine);
            RotateTowardsClosestEnemy();
        }
    }
}
