using System;
using System.Collections;
using System.Collections.Generic;
using CustomEventArgs;
using UnityEngine;

public class Turret : Singleton<Turret>
{
    public static event EventHandler<OnShootEventArgs> OnShoot;
    public static event EventHandler OnReloadStart; 
    public static event EventHandler OnReloadEnd;
    public static event Action OnRotateStart;
    public static event Action OnRotateEnd;

    public Queue<Transform> EnemyList;

    [SerializeField] private bool _isEnabled = true;
    
    [Header("")] 
    [SerializeField] private Transform _gunStartPoint;
    [SerializeField] private Transform _gunEndPoint;
    [SerializeField, Range(0.1F, 4F)] private float _reloadTimeInSeconds;

    [Header("")] 
    [SerializeField] private float _rotationSpeed = 2F;

    public bool IsReloading { get; private set; }
    public bool IsShootingLocked { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        EnemyList = new Queue<Transform>();
    }

    private void OnEnable()
    {
        Enemy.OnDestroyEvent += Enemy_OnDestroyEvent;
        TurretRadar.OnEnemySpotted += TurretRadar_OnEnemySpotted;
    }
    
    private void OnDisable()
    {
        Enemy.OnDestroyEvent -= Enemy_OnDestroyEvent;
        TurretRadar.OnEnemySpotted -= TurretRadar_OnEnemySpotted;
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
        
        StartCoroutine(RotateTurretRoutine(EnemyList.Peek()));
    }

    private void Update()
    {
        ShootHandler();
    }

    public void ShootHandler()
    {
        if (!_isEnabled) return;
        if (IsReloading) return;
        if (IsShootingLocked) return;
        if (EnemyList.Count <= 0) return;
        
        OnShoot?.Invoke(this, new OnShootEventArgs {
            GunEndPointPosition = _gunEndPoint.position,
            GunStartPointPosition = _gunStartPoint.position,
        });

        StartCoroutine(ReloadGunRoutine());
    }

    private IEnumerator RotateTurretRoutine(Transform target)
    {
        const float speedMultiplier = 0.25F;
        float step = _rotationSpeed * Time.deltaTime * speedMultiplier;
        Quaternion rotationTarget = Quaternion.LookRotation(target.position - transform.position);
        
        IsShootingLocked = true;
        OnRotateStart?.Invoke();

        while (rotationTarget != transform.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, step);
            step += _rotationSpeed * Time.deltaTime * speedMultiplier;

            yield return null;
        }

        IsShootingLocked = false;
        OnRotateEnd?.Invoke();
    }
    
    private IEnumerator ReloadGunRoutine()
    {
        IsReloading = true;
        
        OnReloadStart?.Invoke(this, EventArgs.Empty);
        
        yield return new WaitForSeconds(_reloadTimeInSeconds);
        
        IsReloading = false;
        
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }
}
