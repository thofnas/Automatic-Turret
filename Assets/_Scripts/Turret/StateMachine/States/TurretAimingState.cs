using System.Collections;
using _Events;
using _Managers;
using UnityEngine;

namespace Turret.StateMachine.States
{
    public class TurretAimingState : TurretBaseState
    {
        public TurretAimingState(TurretStateMachine context, TurretStateFactory turretStateFactory)
            : base(context, turretStateFactory)
        { }

        public override void EnterState()
        {
            Debug.Log("Entered Aiming State.");
            RotateTowardsClosestEnemy();
        }

        public override void ExitState() => Ctx.StopCoroutine(AimTurretRoutine(null));

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates()
        {
            if (!EnemyManager.Instance.HasEnemyInSight())
            {
                SwitchState(Factory.Idle());
            }

            if (Ctx.IsEnemyInFront(EnemyManager.Instance.GetClosestEnemy()))
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
            Ctx.StartCoroutine(AimTurretRoutine(EnemyManager.Instance.GetClosestEnemy().transform));
        }
        
        private IEnumerator AimTurretRoutine(Transform target)
        {
            const float speedMultiplier = 0.25F;
            float step = Ctx.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            Quaternion rotationTarget = Quaternion.LookRotation(target.position - Ctx.transform.position);

            GameEvents.TurretOnAimStart.Invoke();

            while (rotationTarget != Ctx.transform.rotation)
            {
                Ctx.transform.rotation = Quaternion.RotateTowards(Ctx.transform.rotation, rotationTarget, step);
                step += Ctx.TurretRotationSpeed * Time.deltaTime * speedMultiplier;
            
                yield return null;
            }
            
            GameEvents.TurretOnAimEnd.Invoke();
        }
    }
}
