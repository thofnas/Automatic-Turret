using System;
using System.Collections;
using DG.Tweening;
using Events;
using Interfaces;
using Item;
using Managers;
using Turret;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public Guid InstanceID { get; } = Guid.NewGuid();
        
        [SerializeField] private int _maxHealth = 10;
        [SerializeField, Min(0F)] private float _rollSpeed = 1F;
        [SerializeField, Min(0F)] private float _rollDelayInSeconds = 2F;
        [SerializeField, Min(0)] private int _gearsToDrop = 5;
        [SerializeField] private Transform _enemyVisual;
        [SerializeField] private Image _healthBarFillImage;

        private GearSpawner _gearSpawner;
        private Vector3 _enemyAnchorPoint;
        private Vector3 _enemyAxis;
        private bool _isRolling;
        
        public int Health { get; private set; }
        public int GearsToDrop { get => _gearsToDrop; }

        private void Start()
        {
            Health = _maxHealth;
            _healthBarFillImage.fillAmount = 1f;
            
            transform.LookAt(GameManager.Instance.TurretStateMachine.transform);
            
            Vector3 newRotation = transform.eulerAngles;
            newRotation.x = 0;
            newRotation.z = 0;
            transform.eulerAngles = newRotation;
            
            Assemble();
            
            if (UpgradeManager.Instance.GetTurretUpgradedStat(Stat.BulletDamage) >= _maxHealth)
                _healthBarFillImage.transform.parent.gameObject.SetActive(false);
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
            Vector3 turretPosition = GameManager.Instance.TurretStateMachine.transform.position;
            turretPosition.y = transform.position.y;
            
            _enemyAnchorPoint = transform.position
                                + (Vector3.down + (turretPosition - transform.position).normalized) * (transform.localScale.y * 0.5f);
        
            _enemyAxis = Vector3.Cross(Vector3.up, (turretPosition - transform.position).normalized);
        }
        
        private void OnDestroy() => GameEvents.OnEnemyDestroyed.Invoke(this);

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.transform.parent != GameManager.Instance.TurretStateMachine.transform) return;
            
            GameEvents.OnTurretDamaged.Invoke();
            
            Kill();
        }

        public void ApplyDamage(float damage)
        {
            Health -= Mathf.CeilToInt(damage);
            _healthBarFillImage.fillAmount = Mathf.InverseLerp(0f, _maxHealth, Health);

            GameEvents.OnEnemyDamaged.Invoke(this);
            
            if (Health > 0) return;
            
            Kill();
        }

        private void Kill()
        {
            GameEvents.OnEnemyKilled.Invoke(this);
            Destroy(gameObject);
        }

        public Transform GetTransform() => transform;
    }
}

