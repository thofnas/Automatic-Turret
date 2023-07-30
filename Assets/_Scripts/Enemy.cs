using System;
using System.Collections;
using Events;
using Interfaces;
using Managers;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IHaveID
{
    [SerializeField, Min(0F)] private float _rollSpeed = 5F;
    [SerializeField, Min(0F)] private float _rollDelayInSeconds = 2F;
    
    public Guid InstanceID { get; } = Guid.NewGuid();

    private Vector3 _enemyAnchorPoint;
    private Vector3 _enemyAxis;
    private bool _isRolling;

    public bool IsRolling
    {
        get => _isRolling;
        set
        {
            if (_isRolling == value) return;
            
            _isRolling = value;

            if (value)
                GameEvents.OnEnemyRollStart.Invoke(InstanceID);
            else
                GameEvents.OnEnemyRollEnd.Invoke(InstanceID);
        }
    }

    private void Start()
    {
        transform.LookAt(GameManager.Instance.Turret.transform);
        Assemble();
    }

    private void Update()
    {
        if (_isRolling) return;
        StartCoroutine(RollACubeRoutine(_enemyAnchorPoint, _enemyAxis));
    }
    
    private void OnDestroy()
    {
        GameEvents.OnEnemyDestroyed.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.parent == GameManager.Instance.Turret.transform)
            Destroy(gameObject);
    }

    private void Assemble()
    {
        _enemyAnchorPoint = transform.position
                            + (Vector3.down + (GameManager.Instance.Turret.transform.position - transform.position).normalized) * 0.5f;

        _enemyAxis = Vector3.Cross(Vector3.up, (GameManager.Instance.Turret.transform.position - transform.position).normalized);
    }

    public void ApplyDamage()
    {
        Destroy(gameObject);
    }

    public Transform GetTransform() => transform;

    private IEnumerator RollACubeRoutine(Vector3 anchorPoint, Vector3 axis)
    {
    
        IsRolling = true;

        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchorPoint, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01F);
        }
    
        yield return new WaitForSeconds(_rollDelayInSeconds);

        IsRolling = false;
        
        Assemble();
    }
}

