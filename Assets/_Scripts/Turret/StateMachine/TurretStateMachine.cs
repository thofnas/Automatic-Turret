using System;
using System.Linq;
using _Events;
using _Interfaces;
using UnityEngine;

namespace Turret.StateMachine
{
    public class TurretStateMachine : Turret
    {
        // state variables
        private TurretStateFactory _states;
        public TurretBaseState CurrentState { get; set; }

        #region Unity methods
        private void Awake()
        {
            _states = new TurretStateFactory(this);
        }

        private void Start()
        {
            CurrentState = _states.Idle();
            CurrentState.EnterState();
        }

        private void Update() => CurrentState.UpdateState();

        #endregion

        public bool IsEnemyInFront(IDamageable damageable)
        {
            Vector3 direction = transform.forward; // Get the forward direction of the turret
            var turretScannerCollider = TurretScanner.Instance.GetComponent<Collider>();
            float maxDistance = turretScannerCollider.bounds.extents.magnitude; // Set the maximum distance to cast the ray

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, maxDistance);

            return hits.Any(hit => hit.collider.GetComponent<IDamageable>() == damageable);
        }

        public Transform GetTransform() => transform;
    }
}
