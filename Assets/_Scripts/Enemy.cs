using System.Collections;
using _Events;
using _Interfaces;
using _Managers;
using Turret.StateMachine;
using UnityEngine;

public class Enemy : UniqueGameObject, IDamageable
{
    [SerializeField, Min(0F)] private float _rollSpeed = 5F;
    [SerializeField, Min(0F)] private float _rollDelayInSeconds = 2F;

    private Vector3 _enemyAnchorPoint;
    private Vector3 _enemyAxis;
    private bool _isRolling;

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

    private void Assemble()
    {
        _enemyAnchorPoint = transform.position
                            + (Vector3.down + (GameManager.Instance.Turret.transform.position - transform.position).normalized) * 0.5f;

        _enemyAxis = Vector3.Cross(Vector3.up, (GameManager.Instance.Turret.transform.position - transform.position).normalized);
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
    public Transform GetTransform() => transform;

    private void OnDestroy()
    {
        GameEvents.OnEnemyDestroyed.Invoke(InstanceID);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.parent == GameManager.Instance.Turret.transform)
            Destroy(gameObject);
    }

    private IEnumerator RollACubeRoutine(Vector3 anchorPoint, Vector3 axis)
    {
        GameEvents.OnEnemyRollStart.Invoke(InstanceID);
    
        _isRolling = true;

        for (int i = 0; i < (90 / _rollSpeed); i++)
        {
            transform.RotateAround(anchorPoint, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01F);
        }
    
        yield return new WaitForSeconds(_rollDelayInSeconds);
    
        GameEvents.OnEnemyRollEnd.Invoke(InstanceID);
    
        _isRolling = false;
    
        Assemble();
    }
}

