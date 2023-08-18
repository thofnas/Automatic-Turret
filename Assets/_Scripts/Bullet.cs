using System.Collections;
using Interfaces;
using Managers;
using Turret;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private const float LIFE_TIME = 3f;
    private Vector3 _shootDir;
    private IObjectPool<Bullet> _pool;
    private bool _isCollidedWithDamageable;
    private IEnumerator _moveBulletRoutine;

    private void OnEnable()
    {
        _moveBulletRoutine = MoveBulletRoutine();
        StartCoroutine(_moveBulletRoutine);
    }

    private void OnDisable() => StopCoroutine(_moveBulletRoutine);

    private IEnumerator MoveBulletRoutine()
    {
        float bulletSpeed = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.BulletSpeed);
        
        float timePassed = 0;

        while (timePassed < LIFE_TIME)
        {
            transform.position += _shootDir * (Time.deltaTime * bulletSpeed);
            timePassed += Time.deltaTime;
            yield return null;
        }
        
        _pool.Release(this);
    }

    public void Setup(Vector3 shootDir) => _shootDir = shootDir;

    public void SetPool(IObjectPool<Bullet> pool) => _pool = pool;

    public void ResetBullet() => _isCollidedWithDamageable = false;

    private void OnTriggerEnter(Collider bulletCollider)
    {
        if(_isCollidedWithDamageable) return;
        
        var damageable = bulletCollider.GetComponent<IDamageable>();

        if (damageable == null) return;
        
        _isCollidedWithDamageable = true;
        
        _pool.Release(this);
        
        damageable.ApplyDamage(UpgradeManager.Instance.GetTurretUpgradedStat(Stat.BulletDamage));
    }
}

