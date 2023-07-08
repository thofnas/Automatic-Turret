using System;
using System.Collections;
using _CustomEventArgs;
using _Interfaces;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
    {
        public event EventHandler<OnEnemyDestroyEventArgs> OnEnemyDestroyEvent;
        public event EventHandler OnRollStart;
        public event EventHandler OnRollEnd;

        [SerializeField, Min(0F)] private float _rollSpeed = 5F;
        [SerializeField, Min(0F)] private float _rollDelayInSeconds = 2F;
        [SerializeField] private Transform _turretTransform;

        private Vector3 _enemyAnchorPoint;
        private Vector3 _enemyAxis;
        private bool _isRolling;

        private void Start()
        {
            transform.LookAt(_turretTransform);
            Assemble();
        }

        private void Update()
        {
            if (_isRolling) return;
            StartCoroutine(RollACubeRoutine(_enemyAnchorPoint, _enemyAxis));
        }

        private void Assemble()
        {
            _enemyAnchorPoint = transform.position
                                + (Vector3.down + (_turretTransform.position - transform.position).normalized) * 0.5f;

            _enemyAxis = Vector3.Cross(Vector3.up, (_turretTransform.position - transform.position).normalized);
        }

        public void Damage()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            OnEnemyDestroyEvent?.Invoke(this, new OnEnemyDestroyEventArgs {
                Enemy = this
            });
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.transform.parent == _turretTransform)
                Destroy(gameObject);
        }

        private IEnumerator RollACubeRoutine(Vector3 anchorPoint, Vector3 axis)
        {
            OnRollStart?.Invoke(this, EventArgs.Empty);
        
            _isRolling = true;

            for (int i = 0; i < (90 / _rollSpeed); i++)
            {
                transform.RotateAround(anchorPoint, axis, _rollSpeed);
                yield return new WaitForSeconds(0.01F);
            }
        
            yield return new WaitForSeconds(_rollDelayInSeconds);
        
            OnRollEnd?.Invoke(this, EventArgs.Empty);
        
            _isRolling = false;
        
            Assemble();
        }
    }

