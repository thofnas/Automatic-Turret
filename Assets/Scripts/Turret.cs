using System;
using System.Collections;
using System.Collections.Generic;
using CustomEventArgs;
using UnityEngine;

public class Turret : Singleton<Turret>
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public event EventHandler OnReloadStart; 
    public event EventHandler OnReloadEnd;
    public Queue<Transform> EnemyList;

    [Header("")] [SerializeField] private bool _isEnabled = true;
    
    [SerializeField] private Transform _gunStartPoint;
    [SerializeField] private Transform _gunEndPoint;
    [SerializeField, Range(0.1F, 4F)] private float _reloadTimeInSeconds;
    [SerializeField] private Transform _enemyTransform;
    [SerializeField] private Transform _turretRadarTransform;

    private bool _isReloading;

    protected override void Awake()
    {
        base.Awake();
        
        EnemyList = new Queue<Transform>();
    }

    private void OnEnable()
    {
        Enemy.OnDestroyEvent += Enemy_OnDestroyEvent;
        _turretRadarTransform.GetComponent<TurretRadar>().OnEnemySpotted += TurretRadar_OnEnemySpotted;
    }
    
    private void OnDisable()
    {
        Enemy.OnDestroyEvent -= Enemy_OnDestroyEvent;
        _turretRadarTransform.GetComponent<TurretRadar>().OnEnemySpotted -= TurretRadar_OnEnemySpotted;
    }

    private void Enemy_OnDestroyEvent()
    {
        EnemyList.Dequeue();
        RotateTowardsClosestEnemy();
    }

    private void TurretRadar_OnEnemySpotted(object sender, EventArgs e)
    {
        RotateTowardsClosestEnemy();
    }

    private void RotateTowardsClosestEnemy()
    {
        if (EnemyList.Count <= 0) return;
        
        transform.LookAt(EnemyList.Peek());
    }

    private void Update()
    {
        if(!_isEnabled) return;
        if (_isReloading) return;
        if (EnemyList.Count <= 0) return;

        Shoot();
    }

    public void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs {
            GunEndPointPosition = _gunEndPoint.position,
            GunStartPointPosition = _gunStartPoint.position,
        });

        StartCoroutine(ReloadGunRoutine());
    }
    
    private IEnumerator ReloadGunRoutine()
    {
        _isReloading = true;
        
        OnReloadStart?.Invoke(this, EventArgs.Empty);
        
        yield return new WaitForSeconds(_reloadTimeInSeconds);
        
        _isReloading = false;
        
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }
}
