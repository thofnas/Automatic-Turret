using System;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Turret.StateMachine
{
    public class TurretStateMachine : Turret
    {
        // state variables
        private TurretStateFactory _states;
        public TurretBaseState CurrentState { get; set; }
        
        public void Initialize()
        {
            _states = new TurretStateFactory(this);
            
            CurrentState = _states.Idle();
            CurrentState.EnterState();
        }

        #region Unity methods
        private void Update() => CurrentState.UpdateState();

        #endregion

        public bool IsEnemyInFront(Enemy enemy)
        {
            Vector3 direction = transform.forward; // Get the forward direction of the turret
            var turretScannerCollider = TurretScanner.GetComponent<Collider>();
            float maxDistance = turretScannerCollider.bounds.extents.magnitude; // Set the maximum distance to cast the ray

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, maxDistance);

            return hits.Any(hit => hit.collider.GetComponent<Enemy>() == enemy);
        }

        public Transform GetTransform() => transform;
    }
}
