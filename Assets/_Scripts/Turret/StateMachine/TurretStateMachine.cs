using System.Linq;
using Managers;
using UnityEngine;

namespace Turret.StateMachine
{
    public class TurretStateMachine : Turret
    {
        // state variables
        private TurretStateFactory _states;
        public TurretState CurrentState { get; set; }
        
        public void Initialize()
        {
            _states = new TurretStateFactory(this);
            
            CurrentState = _states.Idle();
            CurrentState.EnterState();
        }

        #region Unity methods
        private void Update() => CurrentState.UpdateState();

        #endregion

        public bool IsEnemyInFront(Enemy.Enemy enemy)
        {
            Vector3 direction = transform.forward; // Get the forward direction of the turret
            float maxDistance = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange); // Set the maximum distance to cast the ray

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, maxDistance);

            return hits.Any(hit => hit.collider.GetComponent<Enemy.Enemy>() == enemy);
        }
        
        public bool IsEnemyInFront()
        {
            Vector3 direction = transform.forward; // Get the forward direction of the turret
            float maxDistance = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.ViewRange); // Set the maximum distance to cast the ray

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, maxDistance);

            return hits.Any(hit => hit.collider.GetComponent<Enemy.Enemy>());
        }

        public Transform GetTransform() => transform;
    }
}
