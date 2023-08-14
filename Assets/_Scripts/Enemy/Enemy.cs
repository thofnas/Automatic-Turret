using System;
using System.Collections;
using DG.Tweening;
using Events;
using Interfaces;
using Managers;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(ItemDropper))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        public Guid InstanceID { get; } = Guid.NewGuid();
        
        [SerializeField, Min(0F)] private float _rollSpeed = 1F;
        [SerializeField, Min(0F)] private float _rollDelayInSeconds = 2F;
        [SerializeField] private Transform _enemyVisual;
        
        private ItemDropper _itemDropper;
        private Vector3 _enemyAnchorPoint;
        private Vector3 _enemyAxis;
        private bool _isRolling;
        
        private void Awake() => _itemDropper = GetComponent<ItemDropper>();

        private void Start()
        {
            transform.LookAt(GameManager.Instance.TurretStateMachine.transform);
            Assemble();
        }
        
        private void Update()
        {
            if (_isRolling) return;
            StartCoroutine(RollACubeRoutine(_enemyAnchorPoint, _enemyAxis));
        }
        
        private IEnumerator RollACubeRoutine(Vector3 anchorPoint, Vector3 axis)
        {
            _isRolling = true;

            for (int i = 0; i < 90 / _rollSpeed; i++)
            {
                Vector3 vector3 = Quaternion.AngleAxis(_rollSpeed, axis) * (transform.position - anchorPoint);
                transform.position = anchorPoint + vector3;
                _enemyVisual.Rotate(Vector3.right, _rollSpeed);
                yield return new WaitForSeconds(0.01F);
            }
        
            yield return new WaitForSeconds(_rollDelayInSeconds);
            
            _isRolling = false;
        
            Assemble();
        }
        
        private void Assemble()
        {
            _enemyAnchorPoint = transform.position
                                + (Vector3.down + (GameManager.Instance.TurretStateMachine.transform.position - transform.position).normalized) * 0.5f;
        
            _enemyAxis = Vector3.Cross(Vector3.up, (GameManager.Instance.TurretStateMachine.transform.position - transform.position).normalized);
        }
        
        private void OnDestroy() => GameEvents.OnEnemyDestroyed.Invoke(this);

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.transform.parent != GameManager.Instance.TurretStateMachine.transform) return;
            GameEvents.OnTurretGotHit.Invoke();
            Destroy(gameObject);
        }

        public void ApplyDamage()
        {
            _itemDropper.DropItems(Ease.Flash, Ease.OutBounce);
            Destroy(gameObject);
        }

        public Transform GetTransform() => transform;
    }
}

